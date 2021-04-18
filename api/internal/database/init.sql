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
