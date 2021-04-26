package routes

import (
	"net/http"

	"github.com/flaviogf/conduit/api/internal/middlewares"
	"github.com/flaviogf/conduit/api/internal/newuser"
	"github.com/flaviogf/conduit/api/internal/session"
	"github.com/gorilla/mux"
)

func NewHandler(r *mux.Router) http.Handler {
	r.Use(middlewares.Json)

	r.HandleFunc("/api/users", newuser.NewUserHandler).Methods(http.MethodPost)

	r.HandleFunc("/api/users/login", session.SessionHandler).Methods(http.MethodPost)

	return r
}
