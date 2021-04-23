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
	row := DB.QueryRowContext(
		ctx,
		`
			SELECT
				a.slug,
				a.title,
				a.description,
				a.body,
				a.created_at,
				a.updated_at,
				u.id author_id,
				u.username author_username,
				u.email author_email,
				u.password_hash author_password_hash,
				u.bio author_bio,
				u.image author_image
			FROM
				articles a
			JOIN
				users u
			ON
				a.author_id = u.id
			WHERE
				slug = $1
		`,
		slug,
	)

	if err := row.Err(); err != nil {
		return nil, err
	}

	var article Article

	var author User

	if err := row.Scan(
		&article.Slug,
		&article.Title,
		&article.Description,
		&article.Body,
		&article.CreatedAt,
		&article.UpdatedAt,
		&author.ID,
		&author.Username,
		&author.Email,
		&author.PasswordHash,
		&author.Bio,
		&author.Image,
	); err != nil {
		return nil, err
	}

	article.Author = &author

	var tags []*Tag

	rows, err := DB.QueryContext(
		ctx,
		`
			SELECT
				t.id,
				t.content
			FROM
				tags t
			JOIN
				article_tags at
			ON
				t.id = at.tag_id
			WHERE
				at.article_slug = $1
		`,
		slug,
	)

	if err != nil {
		return nil, err
	}

	for rows.Next() {
		var tag Tag

		if err := rows.Scan(&tag.ID, &tag.Content); err != nil {
			return nil, err
		}

		tags = append(tags, &tag)
	}

	article.Tags = tags

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

func (a *Article) Delete(ctx context.Context) error {
	_, err := DB.ExecContext(ctx, `DELETE FROM article_tags WHERE article_slug = $1`, a.Slug)

	if err != nil {
		return err
	}

	_, err = DB.ExecContext(ctx, `DELETE FROM articles WHERE slug = $1`, a.Slug)

	if err != nil {
		return err
	}

	return nil
}
