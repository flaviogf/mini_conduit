CREATE TABLE users (
  id serial primary key not null,
  username varchar(250) not null,
  email varchar(250) not null,
  password_hash varchar(250) not null,
  bio varchar(250) not null,
  image varchar(250) not null
);

CREATE TABLE articles (
  slug varchar(250) primary key not null,
  title varchar(250) not null,
  description varchar(250) not null,
  body text not null,
  created_at timestamp with time zone not null,
  updated_at timestamp with time zone not null,
  author_id int not null references users(id)
);

CREATE TABLE article_tags (
  article_slug VARCHAR(250) not null references articles(slug),
  tag VARCHAR(250) not null,
  primary key(article_slug, tag)
);
