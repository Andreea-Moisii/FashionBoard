from datetime import datetime

from pydantic import BaseModel, Field


# --------------- user ----------------
class UserRegister(BaseModel):
    username: str
    password: str
    email: str


class UserLogin(BaseModel):
    username: str
    password: str


class UserOut(BaseModel):
    image_url: str | None = None
    username: str
    email: str
    description: str | None = None


class UserUpdate(BaseModel):
    email: str | None = None
    description: str | None = None
    image_url: str | None = None


# ----------------- Image ----------------
class Image(BaseModel):
    url: str | None = ""
    color1: str | None = ""
    color2: str | None = ""
    color3: str | None = ""


# --------------- Post ----------------
class PostIn(BaseModel):
    price: float
    description: str
    images: list[Image]


class PostOut(BaseModel):
    id_post: int
    user: UserOut | None = None
    price: float
    saves: int
    description: str
    date: datetime
    images: list[Image] | None = None
    saved: bool | None = False

    def __hash__(self):
        return int(self.id_post + self.date.timestamp())


class PostUpdate(BaseModel):
    id: int
    price: float
    description: str
    images_add: list[Image]
    images_remove: list[Image]
