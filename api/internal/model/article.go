package model

import (
	"context"
	"time"
)

type Article struct {
	Slug        string
	Title       string
	Description string
	Body        string
	CreatedAt   *time.Time
	UpdatedAt   *time.Time
	Author      *User
	Tags        []*Tag
}

func NewArticle(slug, title, description, body string, createdAt, updatedAt *time.Time, author *User, tags []*Tag) *Article {
	return &Article{
		Slug:        slug,
		Title:       title,
		Description: description,
		Body:        body,
		CreatedAt:   createdAt,
		UpdatedAt:   updatedAt,
		Author:      author,
		Tags:        tags,
	}
}

func GetArticle(ctx context.Context, slug string) (*Article, error) {
	row := DB.QueryRowContext(ctx, `SELECT slug, title, description, body, created_at, updated_at, author_id FROM articles WHERE slug = $1`, slug)

	if err := row.Err(); err != nil {
		return nil, err
	}

	var article Article

	var authorId int64

	if err := row.Scan(&article.Slug, &article.Title, &article.Description, &article.Body, &article.CreatedAt, &article.UpdatedAt, &authorId); err != nil {
		return nil, err
	}

	return &article, nil
}

func (a *Article) Save(ctx context.Context) error {
	_, err := DB.ExecContext(
		ctx,
		`INSERT INTO articles (slug, title, description, body, created_at, updated_at, author_id) VALUES ($1, $2, $3, $4, $5, $6, $7)`,
		a.Slug,
		a.Title,
		a.Description,
		a.Body,
		a.CreatedAt,
		a.UpdatedAt,
		a.Author.ID,
	)

	if err != nil {
		return err
	}

	_, err = DB.ExecContext(ctx, `DELETE FROM article_tags WHERE article_slug = $1`, a.Slug)

	if err != nil {
		return err
	}

	for _, tag := range a.Tags {
		_, err = DB.ExecContext(ctx, "INSERT INTO article_tags (article_slug, tag_id) VALUES ($1, $2)", a.Slug, tag.ID)

		if err != nil {
			return err
		}
	}

	return nil
}
