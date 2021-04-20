create table users (
  id serial primary key not null,
  username varchar(250) not null,
  email varchar(250) not null,
  password_hash varchar(250) not null,
  bio varchar(250) not null,
  image varchar(250) not null
);

create table user_followers (
  follower_id int not null,
  following_id int not null,
  primary key(follower_id, following_id),
  foreign key(follower_id) references users(id),
  foreign key(following_id) references users(id)
);

create table tags (
  id serial primary key not null,
  content varchar(250) not null
);

create table articles (
  slug varchar(250) primary key not null,
  title varchar(250) not null,
  description varchar(500) not null,
  body text not null,
  created_at timestamp without time zone not null,
  updated_at timestamp without time zone not null,
  author_id int not null,
  foreign key(author_id) references users(id)
);

create table article_tags (
  article_slug varchar(250) not null,
  tag_id int not null,
  primary key(article_slug, tag_id),
  foreign key(article_slug) references articles(slug),
  foreign key(tag_id) references tags(id)
)
