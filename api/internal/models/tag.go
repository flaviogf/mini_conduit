package models

import "context"

func GetTags(ctx context.Context) ([]string, error) {
	sql := `SELECT DISTINCT tag FROM article_tags`

	rows, err := ctx.Value("tx").(Tx).QueryContext(ctx, sql)

	if err != nil {
		return []string{}, err
	}

	tags := make([]string, 0)

	for rows.Next() {
		var tag string

		if err := rows.Scan(&tag); err != nil {
			return []string{}, err
		}

		tags = append(tags, tag)
	}

	return tags, nil
}
