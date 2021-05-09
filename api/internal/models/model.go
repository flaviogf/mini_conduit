package models

import (
	"context"
	"database/sql"
)

var Conn *sql.DB

type Tx interface {
	ExecContext(ctx context.Context, query string, args ...interface{}) (sql.Result, error)
	QueryRowContext(ctx context.Context, query string, args ...interface{}) *sql.Row
	QueryContext(ctx context.Context, query string, args ...interface{}) (*sql.Rows, error)
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
