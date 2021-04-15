package main

import (
	"database/sql"
	"log"
	"net/http"
	"os"

	"github.com/flaviogf/conduit/api/internal/handler"
	"github.com/flaviogf/conduit/api/internal/model"
	"github.com/go-openapi/runtime/middleware"
	"github.com/gorilla/mux"

	_ "github.com/lib/pq"
)

func main() {
	db, err := sql.Open("postgres", os.Getenv("CONDUIT_DATABASE"))

	if err != nil {
		log.Fatal(err)
	}

	model.DB = db

	r := mux.NewRouter()

	r.HandleFunc("/users/login", handler.AuthenticateUserHandler)

	http.Handle("/docs", middleware.SwaggerUI(middleware.SwaggerUIOpts{}, nil))

	http.Handle("/swagger.json", http.FileServer(http.Dir("./")))

	http.Handle("/api", r)

	log.Println(http.ListenAndServe(":3000", nil))
}
