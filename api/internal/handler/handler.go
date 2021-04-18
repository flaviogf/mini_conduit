package handler

import (
	"net/http"

	"github.com/flaviogf/conduit/api/internal/middleware"
	"github.com/gorilla/mux"
)

func NewHandler() http.Handler {
	r := mux.NewRouter()

	r.Use(middleware.Cors)

	r.Use(middleware.Json)

	p := r.NewRoute().Subrouter()

	p.HandleFunc("/api/users/login", AuthenticateUserHandler).Methods(http.MethodPost)

	p.HandleFunc("/api/user", RegisterUserHandler).Methods(http.MethodPost)

	p.HandleFunc("/api/profiles/{username}", GetProfileHandler).Methods(http.MethodGet)

	a := r.NewRoute().Subrouter()

	a.Use(middleware.Authorize)

	a.HandleFunc("/api/user", GetCurrentUserHandler).Methods(http.MethodGet)

	a.HandleFunc("/api/user", UpdateUserHandler).Methods(http.MethodPut)

	a.HandleFunc("/api/profiles/{username}/follow", FollowUserHandler).Methods(http.MethodPost)

	return r
}
