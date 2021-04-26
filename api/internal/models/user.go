package models

import (
	"context"
	"strconv"

	"github.com/dgrijalva/jwt-go"
)

type User struct {
	ID           int64
	Username     string
	Email        string
	PasswordHash string
	Bio          string
	Image        string
}

func NewUser(id int64, username, email, passwordHash, bio, image string) *User {
	return &User{
		ID:           id,
		Username:     username,
		Email:        email,
		PasswordHash: passwordHash,
		Bio:          bio,
		Image:        image,
	}
}

func GetCurrentUser(ctx context.Context) (User, error) {
	id := ctx.Value("userId").(int)

	sql := `SELECT id, username, email, password_hash, bio, image FROM users WHERE id = $1`

	row := ctx.Value("tx").(Tx).QueryRowContext(ctx, sql, id)

	if err := row.Err(); err != nil {
		return User{}, err
	}

	var user User

	if err := row.Scan(&user.ID, &user.Username, &user.Email, &user.PasswordHash, &user.Bio, &user.Image); err != nil {
		return User{}, err
	}

	return user, nil
}

func GetUserByEmail(ctx context.Context, email string) (User, error) {
	sql := `SELECT id, username, email, password_hash, bio, image FROM users WHERE email = $1`

	row := ctx.Value("tx").(Tx).QueryRowContext(ctx, sql, email)

	if err := row.Err(); err != nil {
		return User{}, err
	}

	var user User

	if err := row.Scan(&user.ID, &user.Username, &user.Email, &user.PasswordHash, &user.Bio, &user.Image); err != nil {
		return User{}, err
	}

	return user, nil
}

func (u User) Token(secret string) string {
	claims := jwt.StandardClaims{Subject: strconv.FormatInt(u.ID, 10)}

	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)

	ss, _ := token.SignedString([]byte(secret))

	return ss
}

func (u *User) Save(ctx context.Context) error {
	sql := `INSERT INTO users (username, email, password_hash, bio, image) VALUES ($1, $2, $3, $4, $5) RETURNING id`

	row := ctx.Value("tx").(Tx).QueryRowContext(ctx, sql, u.Username, u.Email, u.PasswordHash, u.Bio, u.Image)

	if err := row.Err(); err != nil {
		return err
	}

	if err := row.Scan(&u.ID); err != nil {
		return err
	}

	return nil
}
