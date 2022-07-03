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


# --------------- post ----------------
class PostIn(BaseModel):
    price: float
    description: str
    images: list[str]
    colors: list[str]


class PostOut(BaseModel):
    id_post: int
    user: UserOut | None = None
    price: float
    saves: int
    description: str
    date: datetime
    colors: list[str] | None = None
    images: list[str] | None = None
    saved: bool | None = False

    def __hash__(self):
        return int(self.id_post + self.date.timestamp())


class PostUpdate(BaseModel):
    price: float
    description: str


# ----------------- color ----------------

class ColorOut(BaseModel):
    code: str
    red: int
    green: int
    blue: int
