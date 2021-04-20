package model

import "context"

type Tag struct {
	ID      int64
	Content string
}

func NewTag(id int64, content string) *Tag {
	return &Tag{
		ID:      id,
		Content: content,
	}
}

func GetTags(ctx context.Context) ([]*Tag, error) {
	rows, err := DB.QueryContext(ctx, `SELECT id, content FROM tags`)

	if err != nil {
		return []*Tag{}, nil
	}

	tags := []*Tag{}

	for rows.Next() {
		var tag Tag

		if err := rows.Scan(&tag.ID, &tag.Content); err != nil {
			return []*Tag{}, err
		}

		tags = append(tags, &tag)
	}

	return tags, nil
}

func (t *Tag) Save(ctx context.Context) error {
	tag, _ := getTagByContent(ctx, t.Content)

	if tag == nil {
		return t.create(ctx)
	}

	t.ID = tag.ID

	return t.update(ctx)
}

func getTagByContent(ctx context.Context, content string) (*Tag, error) {
	row := DB.QueryRowContext(ctx, `SELECT id, content FROM tags WHERE content = $1`, content)

	if err := row.Err(); err != nil {
		return nil, err
	}

	var tag Tag

	if err := row.Scan(&tag.ID, &tag.Content); err != nil {
		return nil, err
	}

	return &tag, nil
}

func (t *Tag) create(ctx context.Context) error {
	row := DB.QueryRowContext(ctx, `INSERT INTO tags (content) VALUES ($1) RETURNING id`, t.Content)

	if err := row.Err(); err != nil {
		return err
	}

	if err := row.Scan(&t.ID); err != nil {
		return err
	}

	return nil
}

func (t *Tag) update(ctx context.Context) error {
	_, err := DB.ExecContext(ctx, `UPDATE tags SET content = $1 WHERE id = $2`, t.Content, t.ID)

	if err != nil {
		return err
	}

	return nil
}
