package models

import (
	"context"
	"time"
)

type Article struct {
	ID          int64
	Slug        string
	Title       string
	Description string
	Body        string
	CreatedAt   time.Time
	UpdatedAt   time.Time
	authorId    int64
}

func NewArticle(id int64, slug, title, description, body string, createdAt, updatedAt time.Time) *Article {
	return &Article{
		ID:          id,
		Slug:        slug,
		Title:       title,
		Description: description,
		Body:        body,
		CreatedAt:   createdAt,
		UpdatedAt:   updatedAt,
	}
}

func GetArticle(ctx context.Context, slug string) (Article, error) {
	sql := `SELECT id, slug, title, description, body, created_at, updated_at, author_id FROM articles WHERE slug = $1`

	row := ctx.Value("tx").(Tx).QueryRowContext(ctx, sql, slug)

	if err := row.Err(); err != nil {
		return Article{}, err
	}

	var article Article

	if err := row.Scan(&article.ID, &article.Slug, &article.Title, &article.Description, &article.Body, &article.CreatedAt, &article.UpdatedAt, &article.authorId); err != nil {
		return Article{}, err
	}

	return article, nil
}

func (a *Article) Save(ctx context.Context) error {
	if a.ID > 0 {
		return a.update(ctx)
	}

	return a.create(ctx)
}

func (a *Article) Destroy(ctx context.Context) error {
	sql := `DELETE FROM articles WHERE id = $1`

	_, err := ctx.Value("tx").(Tx).ExecContext(ctx, sql, a.ID)

	if err != nil {
		return err
	}

	return nil
}

func (a Article) AddTags(ctx context.Context, tags []string) error {
	sql := `INSERT INTO article_tags (article_id, tag) VALUES ($1, $2)`

	for _, tag := range tags {
		_, err := ctx.Value("tx").(Tx).ExecContext(ctx, sql, a.ID, tag)

		if err != nil {
			return err
		}
	}

	return nil
}

func (a Article) AddComment(ctx context.Context, comment *Comment) error {
	commenterId := ctx.Value("userId").(int)

	sql := `INSERT INTO comments (article_id, commenter_id, body, created_at, updated_at) VALUES ($1, $2, $3, $4, $5) RETURNING id`

	row := ctx.Value("tx").(Tx).QueryRowContext(ctx, sql, a.ID, commenterId, comment.Body, comment.CreatedAt, comment.UpdatedAt)

	if err := row.Err(); err != nil {
		return err
	}

	if err := row.Scan(&comment.ID); err != nil {
		return err
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
	sql := `SELECT tag FROM article_tags WHERE article_id = $1`

	rows, err := ctx.Value("tx").(Tx).QueryContext(ctx, sql, a.ID)

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

func (a *Article) create(ctx context.Context) error {
	authorId := ctx.Value("userId").(int)

	sql := `INSERT INTO articles (slug, title, description, body, created_at, updated_at, author_id) VALUES ($1, $2, $3, $4, $5, $6, $7) RETURNING id`

	row := ctx.Value("tx").(Tx).QueryRowContext(ctx, sql, a.Slug, a.Title, a.Description, a.Body, a.CreatedAt, a.UpdatedAt, authorId)

	if err := row.Err(); err != nil {
		return err
	}

	if err := row.Scan(&a.ID); err != nil {
		return err
	}

	a.authorId = int64(authorId)

	return nil
}

func (a *Article) update(ctx context.Context) error {
	sql := `UPDATE articles SET slug = $1, title = $2, description = $3, body = $4, updated_at = $5 WHERE id = $6`

	_, err := ctx.Value("tx").(Tx).ExecContext(ctx, sql, a.Slug, a.Title, a.Description, a.Body, a.UpdatedAt, a.ID)

	if err != nil {
		return err
	}

	return nil
}
