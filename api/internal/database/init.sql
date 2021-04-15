create table users (
  id serial primary key not null,
  username varchar(250) not null,
  email varchar(250) not null,
  password_hash varchar(250) not null,
  bio varchar(250) null,
  image varchar(250) null
)
