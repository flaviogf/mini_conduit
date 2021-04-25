package models

import (
	"context"
	"database/sql"
)

var Conn DB

type DB interface {
	Begin() (*sql.Tx, error)
}

type Tx interface {
	QueryRowContext(ctx context.Context, query string, args ...interface{}) *sql.Row
	Commit() error
	Rollback() error
}

func Context(parent context.Context) (context.Context, error) {
	tx, err := Conn.Begin()

	if err != nil {
		return nil, err
	}

	ctx := context.WithValue(parent, "tx", tx)

	return ctx, nil
}
