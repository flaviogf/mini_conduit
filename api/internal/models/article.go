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

func (a *Article) Save(ctx context.Context) error {
	authorId := ctx.Value("userId").(int)

	sql := `INSERT INTO articles (slug, title, description, body, created_at, updated_at, author_id) VALUES ($1, $2, $3, $4, $5, $6, $7)`

	_, err := ctx.Value("tx").(Tx).ExecContext(ctx, sql, a.Slug, a.Title, a.Description, a.Body, a.CreatedAt, a.UpdatedAt, authorId)

	if err != nil {
		return err
	}

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
