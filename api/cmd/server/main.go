package main

import (
	"database/sql"
	"log"
	"net/http"
	"os"

	"github.com/flaviogf/conduit/api/internal/handler"
	"github.com/flaviogf/conduit/api/internal/middleware"
	"github.com/flaviogf/conduit/api/internal/model"
	openapi "github.com/go-openapi/runtime/middleware"
	"github.com/gorilla/mux"

	_ "github.com/lib/pq"
)

func main() {
	db, err := sql.Open("postgres", os.Getenv("CONDUIT_DATABASE"))

	if err != nil {
		log.Fatal(err)
	}

	model.DB = db

	http.Handle("/docs", openapi.SwaggerUI(openapi.SwaggerUIOpts{}, nil))

	http.Handle("/swagger.json", http.FileServer(http.Dir("./")))

	r := mux.NewRouter()

	r.Use(middleware.Cors)

	r.HandleFunc("/api/users/login", handler.AuthenticateUserHandler).Methods(http.MethodPost)

	http.Handle("/", r)

	log.Println(http.ListenAndServe(":3000", nil))
}
