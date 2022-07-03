import time
from datetime import datetime
from fastapi import FastAPI, Depends, HTTPException, responses, UploadFile, File, Request
from sqlalchemy.orm import Session
from shemas import UserOut, PostOut, PostIn, UserRegister, UserLogin, UserUpdate, PostUpdate, ColorOut
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


def email_exists(email: str, db: Session = Depends(get_db)):
    return db.query(models.User).filter(models.User.email == email).first() is not None


# register user
@app.post("/api/register", status_code=201)
def register(user: UserRegister, db: Session = Depends(get_db)):
    if username_exists(user.username, db):
        raise HTTPException(status_code=400, detail="Username already exists")
    if email_exists(user.email, db):
        raise HTTPException(status_code=400, detail="Email already exists")

    user_model = models.User()
    user_model.username = user.username
    user_model.password = auth_handler.get_password_hash(user.password)
    user_model.email = user.email

    db.add(user_model)
    db.commit()
    return "Register successfully"


# login user
@app.post("/api/login", status_code=200)
def login(user: UserLogin, db: Session = Depends(get_db)):
    user_model = db.query(models.User).filter(models.User.username == user.username).first()
    if not user_model or not auth_handler.verify_password(user.password, user_model.password):
        raise HTTPException(status_code=401, detail="Incorrect username or password")

    token = auth_handler.encode_token(user_model.id_user)
    return {"token": token}


# get a user by username
@app.get("/api/users/{username}", response_model=UserOut)
def get_user(username: str, db: Session = Depends(get_db)):
    user = db.query(models.User).filter(models.User.username == username).first()
    return UserOut(**user.__dict__) if user else None


# update a user
@app.put("/api/users")
def update_user(user: UserUpdate, db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    user_model = db.query(models.User).filter(models.User.id_user == user_id).first()
    if not user_model:
        raise HTTPException(status_code=404, detail="User not found")
    if user.email and email_exists(user.email, db):
        raise HTTPException(status_code=400, detail="Email already exists")

    update_val = {
        "email": user.email if user.email and user.email != "" else user_model.email,
        "description": user.description if user.description and user.description != "" else user_model.description,
        "image_url": user.image_url if user.image_url and user.image_url != "" else user_model.image_url
    }

    db.query(models.User).filter(models.User.id_user == user_id).update(update_val)
    db.commit()
    return


# delete a user
@app.delete("/api/users")
def delete_user(db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    user_model = db.query(models.User).filter(models.User.id_user == user_id).first()
    if not user_model:
        raise HTTPException(status_code=404, detail="User not found")

    db.query(models.User).filter(models.User.id_user == user_id).delete()
    db.commit()
    return


# ============================ POST ============================

def get_post_for_user(user_id: int, post_id: int, db: Session = Depends(get_db)):
    post = db.query(models.Post).filter(models.Post.id_post == post_id).first()
    if not post:
        raise HTTPException(status_code=404, detail="Post not found")
    if post.id_user != user_id:
        raise HTTPException(status_code=403, detail="Unauthorized")
    return post


def filter_search(user_id: int, posts,
                  sortId: int, color: str, word: str,
                  db: Session = Depends(get_db)):
    if word != "":
        posts = posts.filter(models.Post.description.contains(word)
                             | models.User.username.contains(word))
    if color != "":
        posts = posts.filter(models.PostColor.color_cod == closest_color(color, db))

    if sortId == 0:
        posts = posts.order_by(models.Post.date.desc())
    elif sortId == 1:
        posts = posts.order_by(models.Post.saves.desc()).all()
    elif sortId == 2:
        posts = posts.order_by(models.Post.price.asc()).all()
    elif sortId == 3:
        posts = posts.order_by(models.Post.price.desc()).all()
    else:
        raise HTTPException(status_code=400, detail="Invalid sortId")

    return_posts = []
    for post in posts:
        new_post = PostOut(**post[0].__dict__)

        new_post.user = UserOut(**post[1].__dict__)

        colors = db.query(models.PostColor).filter(models.PostColor.id_post == new_post.id_post).all()
        new_post.colors = [color.color_cod for color in colors]

        images = db.query(models.Image).filter(models.Image.id_post == new_post.id_post).all()
        new_post.images = [image.url for image in images]

        # check if the post is liked by the user
        new_post.saved = db.query(models.Save) \
                             .filter(models.Save.id_user == user_id) \
                             .filter(models.Save.id_post == new_post.id_post) \
                             .first() is not None

        return_posts.append(new_post)
    return return_posts


# get all posts
@app.get("/api/posts", response_model=list[PostOut])
def get_posts(db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    posts = db.query(models.Post).all()

    return_posts = []
    for post in posts:
        new_post = PostOut(**post.__dict__)

        user = db.query(models.User).filter(models.User.id_user == post.id_user).first()
        new_post.user = UserOut(**user.__dict__)

        colors = db.query(models.PostColor).filter(models.PostColor.id_post == post.id_post).all()
        new_post.colors = [color.color_cod for color in colors]

        images = db.query(models.Image).filter(models.Image.id_post == post.id_post).all()
        new_post.images = [image.url for image in images]

        # check if the post is liked by the user
        new_post.saved = db.query(models.Save) \
                             .filter(models.Save.id_user == user_id) \
                             .filter(models.Save.id_post == post.id_post) \
                             .first() is not None

        return_posts.append(new_post)

    return return_posts


# get all posts for a user filtered
@app.get("/api/filter/{username}/posts", response_model=list[PostOut])
def get_posts(username: str, sortId: int = 0, color: str = "", word: str = "",
              db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    posts = db.query(models.Post, models.User, models.PostColor) \
        .filter(models.Post.id_user == models.User.id_user) \
        .filter(models.Post.id_post == models.PostColor.id_post) \
        .filter(models.Post.id_post == models.PostColor.id_post) \
        .filter(models.User.username == username)
    # search by word
    return_posts = filter_search(user_id, posts, sortId, color, word, db)

    return list(set(return_posts))


# create a new post
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

    # link color to post
    for color_code in post.colors:
        post_color_model = models.PostColor()
        post_color_model.id_post = post_model.id_post
        post_color_model.color_cod = color_code
        db.add(post_color_model)

    # create the images for the post
    for image_url in post.images:
        image_model = models.Image()
        image_model.id_post = post_model.id_post
        image_model.url = image_url
        db.add(image_model)

    db.commit()
    return "Post created"


# delete a post
@app.delete("/api/posts/{id_post}")
def delete_post(id_post: int, db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    post_model = get_post_for_user(user_id, id_post, db)

    db.query(models.Post).filter(models.Post.id_post == id_post).delete()
    db.commit()
    return "Post deleted"


# update a post
@app.put("/api/posts/{id_post}")
def update_post(id_post: int, post: PostUpdate, db: Session = Depends(get_db),
                user_id: int = Depends(auth_handler.auth_wrapper)):
    post_model = get_post_for_user(user_id, id_post, db)

    update_val = {
        "price": post.price if post.price else post_model.price,
        "description": post.description if post.description and post.description != "" else post_model.description
    }

    db.query(models.Post).filter(models.Post.id_post == id_post).update(update_val)
    db.commit()
    return "Post updated"


# update a post's colors
@app.put("/api/posts/{id_post}/colors")
def update_post_colors(id_post: int, delete_color: list[str], add_colors: list[str],
                       db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    # delete colors
    for color_code in delete_color:
        db.query(models.PostColor) \
            .filter(models.PostColor.id_post == id_post) \
            .filter(models.PostColor.color_cod == color_code) \
            .delete()

    # add colors
    for color_code in add_colors:
        post_color_model = models.PostColor()
        post_color_model.id_post = id_post
        post_color_model.color_cod = color_code
        db.add(post_color_model)

    db.commit()
    return "Post colors updated"


# update a post's images
@app.put("/api/posts/{id_post}/images")
def update_post_images(id_post: int, delete_images: list[int], add_images: list[str],
                       db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    post_model = get_post_for_user(user_id, id_post, db)

    # delete images
    for image_id in delete_images:
        db.query(models.Image) \
            .filter(models.Image.id_post == id_post) \
            .filter(models.Image.id_image == image_id) \
            .delete()

    # add images
    for image_url in add_images:
        image_model = models.Image()
        image_model.id_post = id_post
        image_model.url = image_url
        db.add(image_model)

    db.commit()
    return "Post images updated"


# delete a post
@app.delete("/api/posts/{id_post}")
def delete_post(id_post: int, db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    post_model = get_post_for_user(user_id, id_post, db)

    # delete the post's colors
    db.query(models.PostColor).filter(models.PostColor.id_post == id_post).delete()

    # delete the post's images
    db.query(models.Image).filter(models.Image.id_post == id_post).delete()

    # delete the post
    db.query(models.Post).filter(models.Post.id_post == id_post).delete()
    db.commit()
    return "Post deleted"


# get posts by filters
@app.get("/api/filter/posts")
def get_posts_by_filters(sortId: int = 0, color: str = "", word: str = "",
                         db: Session = Depends(get_db),
                         user_id: int = Depends(auth_handler.auth_wrapper)):
    posts = db.query(models.Post, models.User, models.PostColor) \
        .filter(models.Post.id_user == models.User.id_user) \
        .filter(models.Post.id_post == models.PostColor.id_post)

    return_posts = filter_search(user_id, posts, sortId, color, word, db)

    return list(set(return_posts))


# ============================ COLOR ============================
# get all colors from the database
def getColorTuples(db: Session = Depends(get_db)):
    colors = db.query(models.Color).all()

    return [(color.red, color.green, color.blue) for color in colors]


# return the closest color to the given color
def closest_color(colorHex: str, db: Session = Depends(get_db)):
    color = tuple(int(colorHex[i:i + 2], 16) for i in (0, 2, 4))  # convert hex to int

    colors = np.array(getColorTuples(db))
    color = np.array(color)
    distances = np.sqrt(np.sum((colors - color) ** 2, axis=1))
    index_of_smallest = np.where(distances == np.amin(distances))
    smallest_distance = colors[index_of_smallest]
    r, g, b = smallest_distance[0]

    return ('#%02x%02x%02x' % (r, g, b)).upper()  # convert int to hex


# get all colors
@app.get("/api/colors", response_model=list[ColorOut])
def get_colors(db: Session = Depends(get_db)):
    colors = db.query(models.Color).all()

    return [ColorOut(**color.__dict__) for color in colors]


# ============================ SAVES ============================

# get all saves
@app.get("/api/saves", response_model=list[PostOut])
def get_saves(user_id: int = Depends(auth_handler.auth_wrapper), db: Session = Depends(get_db)):
    saves = db.query(models.Save).filter(models.Save.id_user == user_id).all()

    return_posts = []
    for save in saves:
        post = db.query(models.Post).filter(models.Post.id_post == save.id_post).first()
        new_post = PostOut(**post.__dict__)

        user = db.query(models.User).filter(models.User.id_user == post.id_user).first()
        new_post.user = UserOut(**user.__dict__)

        colors = db.query(models.PostColor).filter(models.PostColor.id_post == post.id_post).all()
        new_post.colors = [color.color_cod for color in colors]

        images = db.query(models.Image).filter(models.Image.id_post == post.id_post).all()
        new_post.images = [image.url for image in images]

        new_post.saved = True

        return_posts.append(new_post)

    return return_posts


# add a save
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

    # update the post's save count
    db.query(models.Post) \
        .filter(models.Post.id_post == id_post) \
        .update({"saves": models.Post.saves + 1})

    db.commit()
    return "Save added"


# delete a save
@app.delete("/api/saves/{id_post}")
def delete_save(id_post: int, db: Session = Depends(get_db), user_id: int = Depends(auth_handler.auth_wrapper)):
    db.query(models.Save) \
        .filter(models.Save.id_post == id_post) \
        .filter(models.Save.id_user == user_id) \
        .delete()

    # update the post's save count
    db.query(models.Post) \
        .filter(models.Post.id_post == id_post) \
        .update({"saves": models.Post.saves - 1})

    db.commit()
    return "Save deleted"


# get filtered saves
@app.get("/api/filter/saves")
def get_saves_by_filters(sortId: int = 0, color: str = "", word: str = "",
                         db: Session = Depends(get_db),
                         user_id: int = Depends(auth_handler.auth_wrapper)):
    saves = db.query(models.Post, models.User, models.PostColor, models.Save) \
        .filter(models.Save.id_user == models.User.id_user) \
        .filter(models.Save.id_post == models.Post.id_post) \
        .filter(models.Post.id_post == models.PostColor.id_post) \
        .filter(models.Post.id_user == user_id)

    return_posts = filter_search(user_id, saves, sortId, color, word, db)

    return list(set(return_posts))


# ============================ IMAGES ============================
# return an image
@app.get("/api/images/{image_name}")
def get_image(image_name: str):
    # return the image from images folder
    return responses.FileResponse(f"images/{image_name}")


@app.post("/api/images")
def add_image(request: Request, image: UploadFile = File(...), ):
    # save the image to the images folder
    print("here")
    if _is_image(image.filename):
        timestr = time.strftime("%Y%m%d-%H%M%S")
        image_name = f"{timestr}-{image.filename.replace(' ', '-')}"
        with open(f"images/{image_name}", "wb+") as f:
            f.write(image.file.read())

        return f"http://{request.url.hostname}:{request.url.port}{request.url.path}/{image_name}"
    else:
        raise HTTPException(status_code=400, detail="Invalid image")


def _is_image(filename: str) -> bool:
    print(filename)
    return filename.endswith((".jpg", ".jpeg", ".png", ".gif"))
