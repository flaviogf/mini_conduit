package model

import (
	"context"
	"os"
	"strconv"

	"github.com/dgrijalva/jwt-go"
	"golang.org/x/crypto/bcrypt"
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

func GetUserByEmail(ctx context.Context, email string) (*User, error) {
	row := DB.QueryRowContext(ctx, "SELECT id, username, email, password_hash, bio, image FROM users WHERE email = $1", email)

	if err := row.Err(); err != nil {
		return nil, err
	}

	var user User

	if err := row.Scan(&user.ID, &user.Username, &user.Email, &user.PasswordHash, &user.Bio, &user.Image); err != nil {
		return nil, err
	}

	return &user, nil
}

func (u User) Attempt(password string) (string, error) {
	err := bcrypt.CompareHashAndPassword([]byte(u.PasswordHash), []byte(password))

	if err != nil {
		return "", err
	}

	claims := jwt.StandardClaims{
		Subject: strconv.FormatInt(u.ID, 10),
	}

	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)

	ss, err := token.SignedString([]byte(os.Getenv("CONDUIT_KEY")))

	if err != nil {
		return "", err
	}

	return ss, nil
}

func (u *User) Save(ctx context.Context) error {
	row := DB.QueryRowContext(ctx, `INSERT INTO users (username, email, password_hash, bio, image) VALUES ($1, $2, $3, $4, $5) RETURNING id`, u.Username, u.Email, u.PasswordHash, u.Bio, u.Image)

	if err := row.Err(); err != nil {
		return err
	}

	if err := row.Scan(&u.ID); err != nil {
		return err
	}

	return nil
}
