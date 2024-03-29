CREATE TABLE users (
  id serial primary key not null,
  username varchar(250) unique not null,
  email varchar(250) unique not null,
  password_hash varchar(250) not null,
  bio varchar(250) not null,
  image varchar(250) not null
);

CREATE TABLE articles (
  id serial primary key not null,
  slug varchar(250) unique not null,
  title varchar(250) not null,
  description varchar(250) not null,
  body text not null,
  created_at timestamp with time zone not null,
  updated_at timestamp with time zone not null,
  author_id int not null references users(id)
);

CREATE TABLE article_tags (
  article_id int not null references articles(id) on delete cascade,
  tag varchar(250) not null,
  primary key(article_id, tag)
);

CREATE TABLE comments (
  id serial primary key not null,
  article_id int not null references articles(id) on delete cascade,
  commenter_id int not null references users(id) on delete cascade,
  body varchar(250) not null,
  created_at timestamp with time zone not null,
  updated_at timestamp with time zone not null
)
