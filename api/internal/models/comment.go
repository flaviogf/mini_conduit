package models

import (
	"context"
	"time"
)

type Comment struct {
	ID          int64
	Body        string
	CreatedAt   time.Time
	UpdatedAt   time.Time
	commenterId int64
}

func NewComment(id int64, body string, createdAt, updatedAt time.Time) *Comment {
	return &Comment{
		ID:        id,
		Body:      body,
		CreatedAt: createdAt,
		UpdatedAt: updatedAt,
	}
}

func (c Comment) GetAuthor(ctx context.Context) (User, error) {
	sql := `SELECT id, username, email, password_hash, bio, image FROM users WHERE id = $1`

	row := ctx.Value("tx").(Tx).QueryRowContext(ctx, sql, c.commenterId)

	if err := row.Err(); err != nil {
		return User{}, err
	}

	var user User

	if err := row.Scan(&user.ID, &user.Username, &user.Email, &user.PasswordHash, &user.Bio, &user.Image); err != nil {
		return User{}, err
	}

	return user, nil
}
