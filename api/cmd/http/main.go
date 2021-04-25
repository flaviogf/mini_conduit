package main

import (
	"database/sql"
	"log"
	"net/http"
	"os"

	"github.com/flaviogf/conduit/api/internal/models"
	"github.com/flaviogf/conduit/api/internal/routes"
	"github.com/go-openapi/runtime/middleware"
	"github.com/gorilla/mux"

	_ "github.com/lib/pq"
)

func main() {
	db, err := sql.Open("postgres", os.Getenv("CONDUIT_DB"))

	if err != nil {
		log.Fatal(err)
	}

	models.Conn = db

	r := mux.NewRouter()

	http.Handle("/", routes.NewHandler(r))

	http.Handle("/docs", middleware.SwaggerUI(middleware.SwaggerUIOpts{SpecURL: "swagger.yaml"}, nil))

	http.Handle("/swagger.yaml", http.FileServer(http.Dir("./")))

	log.Println(http.ListenAndServe(":80", nil))
}
