from sqlalchemy import Column, Integer, String, ForeignKey, Float, DateTime
from database import Base


class User(Base):
    __tablename__ = "user"
    id_user = Column(Integer, primary_key=True, index=True)
    image_url = Column(String)
    username = Column(String, unique=True, index=True)
    password = Column(String)
    email = Column(String, unique=True, index=True)
    description = Column(String)


class Image(Base):
    __tablename__ = "image"
    id_image = Column(Integer, primary_key=True, index=True)
    id_post = Column(Integer, ForeignKey("post.id_post"))
    url = Column(String)


class Post(Base):
    __tablename__ = "post"
    id_post = Column(Integer, primary_key=True, index=True)
    id_user = Column(Integer, ForeignKey("user.id_user"))
    price = Column(Float)
    saves = Column(Integer)
    description = Column(String)
    date = Column(DateTime)


class Save(Base):
    __tablename__ = "save"
    id_save = Column(Integer, primary_key=True, index=True)
    id_user = Column(Integer, ForeignKey("user.id_user"))
    id_post = Column(Integer, ForeignKey("post.id_post"))


class Color(Base):
    __tablename__ = "color"
    code = Column(String, primary_key=True, index=True)
    red = Column(Integer)
    green = Column(Integer)
    blue = Column(Integer)


class PostColor(Base):
    __tablename__ = "post_color"
    id_post_color = Column(Integer, primary_key=True, index=True)
    color_cod = Column(String, ForeignKey("color.code"))
    id_post = Column(Integer, ForeignKey("post.id_post"))
