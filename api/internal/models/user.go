package models

import "context"

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
