package routes

import (
	"net/http"

	"github.com/flaviogf/conduit/api/internal/currentuser"
	"github.com/flaviogf/conduit/api/internal/middlewares"
	"github.com/flaviogf/conduit/api/internal/newuser"
	"github.com/flaviogf/conduit/api/internal/session"
	"github.com/flaviogf/conduit/api/internal/updateuser"
	"github.com/gorilla/mux"
)

func NewHandler(r *mux.Router) http.Handler {
	r.Use(middlewares.Json)

	p := r.NewRoute().Subrouter()

	p.HandleFunc("/api/users", newuser.NewUserHandler).Methods(http.MethodPost)

	p.HandleFunc("/api/users/login", session.SessionHandler).Methods(http.MethodPost)

	a := r.NewRoute().Subrouter()

	a.Use(middlewares.Authorize)

	a.HandleFunc("/api/users", currentuser.CurrentUserHandler).Methods(http.MethodGet)

	a.HandleFunc("/api/users", updateuser.UpdateUserHandler).Methods(http.MethodPut)

	return r
}
