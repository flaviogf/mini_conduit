package models

import (
	"context"
	"time"
)

type Article struct {
	Slug        string
	Title       string
	Description string
	Body        string
	CreatedAt   time.Time
	UpdatedAt   time.Time
	authorId    int64
}

func NewArticle(slug, title, description, body string, createdAt, updatedAt time.Time) *Article {
	return &Article{
		Slug:        slug,
		Title:       title,
		Description: description,
		Body:        body,
		CreatedAt:   createdAt,
		UpdatedAt:   updatedAt,
	}
}

func GetArticle(ctx context.Context, slug string) (Article, error) {
	sql := `SELECT slug, title, description, body, created_at, updated_at, author_id FROM articles WHERE slug = $1`

	row := ctx.Value("tx").(Tx).QueryRowContext(ctx, sql, slug)

	if err := row.Err(); err != nil {
		return Article{}, err
	}

	var article Article

	if err := row.Scan(&article.Slug, &article.Title, &article.Description, &article.Body, &article.CreatedAt, &article.UpdatedAt, &article.authorId); err != nil {
		return Article{}, err
	}

	return article, nil
}

func (a *Article) Save(ctx context.Context) error {
	authorId := ctx.Value("userId").(int)

	sql := `INSERT INTO articles (slug, title, description, body, created_at, updated_at, author_id) VALUES ($1, $2, $3, $4, $5, $6, $7)`

	_, err := ctx.Value("tx").(Tx).ExecContext(ctx, sql, a.Slug, a.Title, a.Description, a.Body, a.CreatedAt, a.UpdatedAt, authorId)

	if err != nil {
		return err
	}

	a.authorId = int64(authorId)

	return nil
}

func (a Article) AddTags(ctx context.Context, tags []string) error {
	sql := `INSERT INTO article_tags (article_slug, tag) VALUES ($1, $2)`

	for _, tag := range tags {
		_, err := ctx.Value("tx").(Tx).ExecContext(ctx, sql, a.Slug, tag)

		if err != nil {
			return err
		}
	}

	return nil
}

func (a Article) GetAuthor(ctx context.Context) (User, error) {
	sql := `SELECT id, username, email, password_hash, bio, image FROM users WHERE id = $1`

	row := ctx.Value("tx").(Tx).QueryRowContext(ctx, sql, a.authorId)

	if err := row.Err(); err != nil {
		return User{}, err
	}

	var user User

	if err := row.Scan(&user.ID, &user.Username, &user.Email, &user.PasswordHash, &user.Bio, &user.Image); err != nil {
		return User{}, err
	}

	return user, nil
}

func (a Article) GetTags(ctx context.Context) ([]string, error) {
	sql := `SELECT tag FROM article_tags WHERE article_slug = $1`

	rows, err := ctx.Value("tx").(Tx).QueryContext(ctx, sql, a.Slug)

	if err != nil {
		return nil, err
	}

	tags := make([]string, 0)

	for rows.Next() {
		var tag string

		if err := rows.Scan(&tag); err != nil {
			return nil, err
		}

		tags = append(tags, tag)
	}

	return tags, nil
}
