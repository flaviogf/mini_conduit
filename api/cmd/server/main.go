package main

import (
	"database/sql"
	"log"
	"net/http"
	"os"

	"github.com/flaviogf/conduit/api/internal/handler"
	"github.com/flaviogf/conduit/api/internal/model"
	"github.com/go-openapi/runtime/middleware"

	_ "github.com/lib/pq"
)

func main() {
	db, err := sql.Open("postgres", os.Getenv("CONDUIT_DATABASE"))

	if err != nil {
		log.Fatal(err)
	}

	model.DB = db

	http.Handle("/docs", middleware.SwaggerUI(middleware.SwaggerUIOpts{}, nil))

	http.Handle("/swagger.json", http.FileServer(http.Dir("./")))

	http.Handle("/", handler.NewHandler())

	log.Println(http.ListenAndServe(":3000", nil))
}
