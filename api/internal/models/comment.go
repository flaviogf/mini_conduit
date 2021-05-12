package models

import "time"

type Comment struct {
	ID        int64
	Body      string
	CreatedAt time.Time
	UpdatedAt time.Time
}

func NewComment(id int64, body string, createdAt, updatedAt time.Time) *Comment {
	return &Comment{
		ID:        id,
		Body:      body,
		CreatedAt: createdAt,
		UpdatedAt: updatedAt,
	}
}
