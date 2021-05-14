package routes

import (
	"net/http"

	"github.com/flaviogf/conduit/api/internal/addcomment"
	"github.com/flaviogf/conduit/api/internal/currentuser"
	"github.com/flaviogf/conduit/api/internal/deletearticle"
	"github.com/flaviogf/conduit/api/internal/deletecomment"
	"github.com/flaviogf/conduit/api/internal/getarticle"
	"github.com/flaviogf/conduit/api/internal/getcomments"
	"github.com/flaviogf/conduit/api/internal/middlewares"
	"github.com/flaviogf/conduit/api/internal/newarticle"
	"github.com/flaviogf/conduit/api/internal/newuser"
	"github.com/flaviogf/conduit/api/internal/session"
	"github.com/flaviogf/conduit/api/internal/updatearticle"
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

	a.HandleFunc("/api/articles", newarticle.NewArticleHandler).Methods(http.MethodPost)

	a.HandleFunc("/api/articles/{slug}", getarticle.GetArticleHandler).Methods(http.MethodGet)

	a.HandleFunc("/api/articles/{slug}", updatearticle.UpdateArticleHandler).Methods(http.MethodPut)

	a.HandleFunc("/api/articles/{slug}", deletearticle.DeleteArticleHandler).Methods(http.MethodDelete)

	a.HandleFunc("/api/articles/{slug}/comments", addcomment.AddCommentHandler).Methods(http.MethodPost)

	a.HandleFunc("/api/articles/{slug}/comments", getcomments.GetCommentsHandler).Methods(http.MethodGet)

	a.HandleFunc("/api/articles/{slug}/comments/{id}", deletecomment.DeleteCommentHandler).Methods(http.MethodDelete)

	return r
}
