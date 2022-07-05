import os
import time
from datetime import datetime
from fastapi import FastAPI, Depends, HTTPException, responses, UploadFile, File, Request
from sqlalchemy.orm import Session
from sqlalchemy import desc
from shemas import UserOut, PostOut, PostIn, UserRegister, UserLogin, UserUpdate, PostUpdate, Image
from database import engine, SessionLocal
from auth import AuthHandler
import models
import numpy as np

app = FastAPI()

# create database and tables if they don't exist
models.Base.metadata.create_all(bind=engine)
auth_handler = AuthHandler()


# get the database session
def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()


# ============================ USER ============================
def username_exists(username: str, db: Session = Depends(get_db)):
    return db.query(models.User).filter(models.User.username == username).first() is not None


def email_exists(email: str, user_id: int, db: Session = Depends(get_db)):
    return db.query(models.User).filter((models.User.email == email) &
                                        (models.User.id_user != user_id)).first() is not None


# // ------------------ register a new user ------------------ //
@app.post("/api/register", status_code=201)
def register(user: UserRegister, db: Session = Depends(get_db)):
    if username_exists(user.username, db):
        raise HTTPException(status_code=400, detail="Username already exists")
    if email_exists(user.email, -1, db):
        raise HTTPException(status_code=400, detail="Email already exists")

    user_model = models.User()
    user_model.username = user.username
    user_model.password = auth_handler.get_password_hash(user.password)
    user_model.email = user.email

    db.add(user_model)
    db.commit()
    return "Register successfully"


# // ------------------ login user ------------------ //
@app.post("/api/login", status_code=200)
def login(user: UserLogin, db: Session = Depends(get_db)):
    user_model = db.query(models.User).filter(models.User.username == user.username).first()
    if not user_model or not auth_handler.verify_password(user.password, user_model.password):
        raise HTTPException(status_code=401, detail="Incorrect username or password")

    token = auth_handler.encode_token(user_model.id_user)
    return {"token": token}


# // ------------------ get user data ------------------ //
@app.get("/api/users/{username}", response_model=UserOut)
def get_user(username: str, db: Session = Depends(get_db)):
    user = db.query(models.User).filter(models.User.username == username).first()
    return UserOut(**user.__dict__) if user else None


# // ------------------ update user data ------------------ //
@app.put("/api/users")
def update_user(user: UserUpdate, db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    user_model = db.query(models.User).filter(models.User.id_user == user_id).first()
    if not user_model:
        raise HTTPException(status_code=404, detail="User not found")
    if user.email and email_exists(user.email, user_id, db):
        raise HTTPException(status_code=400, detail="Email already exists")

    # delete old profile image if it exists
    if user.image_url != user_model.image_url and user_model.image_url:
        path_image = f"images/{user_model.image_url[user_model.image_url.rfind('/') + 1:]}"
        if os.path.exists(path_image):
            os.remove(path_image)

    update_val = {
        "email": user.email if user.email and user.email != "" else user_model.email,
        "description": user.description if user.description and user.description != "" else user_model.description,
        "image_url": user.image_url if user.image_url and user.image_url != "" else user_model.image_url
    }

    db.query(models.User).filter(models.User.id_user == user_id).update(update_val)
    db.commit()
    return


# // ------------------ delete user ------------------ //
@app.delete("/api/users")
def delete_user(db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    user_model = db.query(models.User).filter(models.User.id_user == user_id).first()
    if not user_model:
        raise HTTPException(status_code=404, detail="User not found")

    # get all posts of the user
    posts = db.query(models.Post).filter(models.Post.id_user == user_id).all()

    for post in posts:
        # get all images of the post
        images = db.query(models.Image).filter(models.Image.id_post == post.id_post).all()
        for image in images:
            # delete image from disk
            image_path = f"images/{image.url[image.url.rfind('/') + 1:]}"
            if os.path.exists(image_path):
                os.remove(image_path)
            # delete image from database
            db.query(models.Image).filter(models.Image.id_image == image.id_image).delete()

        # delete saved post from database
        db.query(models.Post).filter(models.Post.id_post == post.id_post).delete()

        # delete post from database
        db.query(models.Post).filter(models.Post.id_post == post.id_post).delete()

    # delete user profile image from disk
    image_path = f"images/{user_model.image_url[user_model.image_url.rfind('/') + 1:]}"
    if os.path.exists(image_path):
        os.remove(image_path)

    # delete user from database
    db.query(models.User).filter(models.User.id_user == user_id).delete()
    db.commit()
    return "User deleted"


# ============================ POST ============================
# // ------------------ get a post for a user ------------------ //
def get_post_for_user(user_id: int, post_id: int, db: Session = Depends(get_db)):
    post = db.query(models.Post).filter(models.Post.id_post == post_id).first()
    if not post:
        raise HTTPException(status_code=404, detail="Post not found")
    if post.id_user != user_id:
        raise HTTPException(status_code=403, detail="Unauthorized")
    return post


# // ------------------ filter the search results ------------------ //
def filter_search(search, user_id: int, sortId: int, color: str, word: str,
                  db: Session = Depends(get_db)):
    if word != "":
        search = search.filter(models.Post.description.contains(word)
                               | models.User.username.contains(word))
    if color != "":
        close_color = closest_color(color, db)
        search = search.filter((models.Image.color1 == close_color) |
                               (models.Image.color2 == close_color) |
                               (models.Image.color3 == close_color))

    if sortId == 0:
        search = search.order_by(desc(models.Post.date)).distinct(models.Post.id_post).all()
    elif sortId == 1:
        search = search.order_by(models.Post.saves.desc()).distinct(models.Post.id_post).all()
    elif sortId == 2:
        search = search.order_by(models.Post.price).distinct(models.Post.id_post).all()
    elif sortId == 3:
        search = search.order_by(models.Post.price.desc()).distinct(models.Post.id_post).all()
    else:
        raise HTTPException(status_code=400, detail="Invalid sortId")

    return_search = []
    for post in search:
        new_post = PostOut(**post[0].__dict__)
        new_post.user = UserOut(**post[1].__dict__)

        images = db.query(models.Image).filter(models.Image.id_post == new_post.id_post).all()
        new_post.images = [Image(**image.__dict__) for image in images]

        # check if the post is liked by the users
        new_post.saved = db.query(models.Save) \
                           .filter(models.Save.id_user == user_id) \
                           .filter(models.Save.id_post == new_post.id_post) \
                           .first() is not None

        return_search.append(new_post)
    return return_search


# // ------------------ get posts for a user ------------------ //
@app.get("/api/filter/{username}/posts", response_model=list[PostOut])
def get_search(username: str, sortId: int = 0, color: str = "", word: str = "",
               db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    search = db.query(models.Post, models.User, models.Image) \
        .filter(models.Post.id_user == models.User.id_user) \
        .filter(models.Post.id_post == models.Image.id_post) \
        .filter(models.User.username == username)

    return_search = filter_search(search, user_id, sortId, color, word, db)

    return return_search


# // ------------------ create a post ------------------ //
@app.post("/api/posts")
def create_post(post: PostIn, db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    post_model = models.Post()
    post_model.id_user = user_id
    post_model.price = post.price
    post_model.description = post.description
    post_model.date = datetime.now()
    post_model.saves = 0

    # create the post in the database
    db.add(post_model)
    # put the id in the post
    db.flush()

    # add images to the database
    for image in post.images:
        post_image = models.Image()
        post_image.id_post = post_model.id_post
        post_image.url = image.url
        post_image.color1 = closest_color(image.color1, db)
        post_image.color2 = closest_color(image.color2, db)
        post_image.color3 = closest_color(image.color3, db)
        db.add(post_image)

    db.commit()
    return "Post created"


#  // ------------------ delete a post ------------------ //
@app.delete("/api/posts/{id_post}")
def delete_post(id_post: int, db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    get_post_for_user(user_id, id_post, db)
    # delete all images for the post and for the disk
    images = db.query(models.Image).filter(models.Image.id_post == id_post).all()
    for image in images:
        image_path = f"images/{image.url[image.url.rfind('/') + 1:]}"
        if os.path.exists(image_path):
            os.remove(image_path)
        db.query(models.Image).filter(models.Image.id_image == image.id_image).delete()
    # delete the saves for the post
    db.query(models.Save).filter(models.Save.id_post == id_post).delete()
    # delete the post
    db.query(models.Post).filter(models.Post.id_post == id_post).delete()
    db.commit()
    return "Post deleted"


# // ------------------ update a post ------------------ //
@app.put("/api/posts/{id_post}")
def update_post(id_post: int, post: PostUpdate, db: Session = Depends(get_db),
                user_id: int = Depends(auth_handler.auth_wrapper)):
    post_model = get_post_for_user(user_id, id_post, db)

    update_val = {
        "price": post.price if post.price else post_model.price,
        "description": post.description if post.description and post.description != "" else post_model.description
    }
    db.query(models.Post).filter(models.Post.id_post == id_post).update(update_val)

    # remove images from the database
    for image in post.images_remove:
        db.query(models.Image).filter(models.Image.url == image.url).delete()
        # delete the image from images folder
        image_path = f"images/{image.url[image.url.rfind('/') + 1:]}"
        if os.path.exists(image_path):
            os.remove(image_path)

    # add images to the database
    for image in post.images_add:
        post_image = models.Image()
        post_image.id_post = post_model.id_post
        post_image.url = image.url
        post_image.color1 = closest_color(image.color1, db)
        post_image.color2 = closest_color(image.color2, db)
        post_image.color3 = closest_color(image.color3, db)
        db.add(post_image)

    db.commit()
    return "Post updated"


# // ------------------ get all posts ------------------ //
@app.get("/api/filter/posts", response_model=list[PostOut])
def get_posts_by_filters(sortId: int = 0, color: str = "", word: str = "",
                         db: Session = Depends(get_db),
                         user_id: int = Depends(auth_handler.auth_wrapper)):
    search = db.query(models.Post, models.User, models.Image) \
        .filter(models.Post.id_user == models.User.id_user) \
        .filter(models.Post.id_post == models.Image.id_post)

    return_search = filter_search(search, user_id, sortId, color, word, db)

    # return list(set(return_search))
    return return_search


# ============================ COLOR ============================
# get all colors from the database
def getColorTuples(db: Session = Depends(get_db)):
    colors = db.query(models.Color).all()

    return [(color.red, color.green, color.blue) for color in colors]


# return the closest color to the given colors
def closest_color(colorHex: str, db: Session = Depends(get_db)):
    # remove # from hex if it exists
    colorHex = colorHex.replace("#", "")
    color = tuple(int(colorHex[i:i + 2], 16) for i in (0, 2, 4))  # convert hex to int values

    colors = np.array(getColorTuples(db))
    color = np.array(color)
    distances = np.sqrt(np.sum((colors - color) ** 2, axis=1))
    index_of_smallest = np.where(distances == np.amin(distances))
    smallest_distance = colors[index_of_smallest]
    r, g, b = smallest_distance[0]

    return ('#%02x%02x%02x' % (r, g, b)).upper()  # convert int to hex


# ============================ SAVES ============================


# // ------------------ add a save ------------------ //
@app.post("/api/saves/{id_post}")
def add_save(id_post: int, db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    # check if the save exists
    save = db.query(models.Save) \
        .filter(models.Save.id_user == user_id) \
        .filter(models.Save.id_post == id_post).first()

    if save:
        raise HTTPException(status_code=400, detail="Save already exists")

    save_model = models.Save()
    save_model.id_user = user_id
    save_model.id_post = id_post
    db.add(save_model)

    # update the post's saved count
    db.query(models.Post) \
        .filter(models.Post.id_post == id_post) \
        .update({"saves": models.Post.saves + 1})

    db.commit()
    return "Save added"


# // ------------------ delete a save ------------------ //
@app.delete("/api/saves/{id_post}")
def delete_save(id_post: int, db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    save = db.query(models.Save) \
               .filter(models.Save.id_user == user_id) \
               .filter(models.Save.id_post == id_post).first() is not None

    if not save:
        raise HTTPException(status_code=404, detail="No save found")

    db.query(models.Save) \
        .filter(models.Save.id_post == id_post) \
        .filter(models.Save.id_user == user_id) \
        .delete()

    # update the post's saved count
    db.query(models.Post) \
        .filter(models.Post.id_post == id_post) \
        .update({"saves": models.Post.saves - 1})

    db.commit()
    return "Save deleted"


# // ------------------ get all saves ------------------ //
@app.get("/api/filter/saves")
def get_saves_by_filters(sortId: int = 0, color: str = "", word: str = "",
                         db: Session = Depends(get_db),
                         user_id: int = Depends(auth_handler.auth_wrapper)):
    saves = db.query(models.Post, models.User, models.Image, models.Save) \
        .filter(models.Post.id_user == models.User.id_user) \
        .filter(models.Post.id_post == models.Image.id_post) \
        .filter(models.Post.id_post == models.Save.id_post) \
        .filter(models.Save.id_user == user_id)

    return_search = filter_search(saves, user_id, sortId, color, word, db)

    return return_search


# ============================ IMAGES ============================
# // ------------------ get a image ------------------ //
@app.get("/api/images/{image_name}")
def get_image(image_name: str):
    # return the image from images folder
    return responses.FileResponse(f"images/{image_name}")


# // ------------------ add a image ------------------ //
@app.post("/api/images", response_model=Image)
def add_image(request: Request, image: UploadFile = File(...), ):
    # save the image to the images folder
    print("here")
    if _is_image(image.filename):
        timestr = time.strftime("%Y%m%d-%H%M%S")
        image_name = f"{timestr}-{image.filename.replace(' ', '-')}"
        with open(f"images/{image_name}", "wb+") as f:
            f.write(image.file.read())

        image = Image()
        # noinspection HttpUrlsUsage
        image.url = f'http://{request.url.hostname}:{request.url.port}{request.url.path}/{image_name}'
        image.color1 = ""
        image.color2 = ""
        image.color3 = ""
        return image
    else:
        raise HTTPException(status_code=400, detail="Invalid image")


def _is_image(filename: str) -> bool:
    print(filename)
    return filename.endswith((".jpg", ".jpeg", ".png", ".gif"))
