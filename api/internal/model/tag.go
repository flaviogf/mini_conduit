package model

import "context"

type Tag struct {
	ID      int64
	Content string
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
