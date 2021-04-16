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

	r.HandleFunc("/api/users/login", AuthenticateUserHandler).Methods(http.MethodPost)

	r.HandleFunc("/api/user", RegisterUserHandler).Methods(http.MethodPost)

	return r
}
