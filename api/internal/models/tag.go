package models

import "context"

type Tag string

func GetTags(ctx context.Context) ([]Tag, error) {
	sql := `SELECT DISTINCT body FROM article_tags`

	rows, err := ctx.Value("tx").(Tx).QueryContext(ctx, sql)

	if err != nil {
		return []Tag{}, err
	}

	tags := make([]Tag, 0)

	for rows.Next() {
		var tag Tag

		if err := rows.Scan(&tag); err != nil {
			return []Tag{}, err
		}

		tags = append(tags, tag)
	}

	return tags, nil
}
