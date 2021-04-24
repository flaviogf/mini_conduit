package main

import (
	"log"
	"net/http"

	"github.com/go-openapi/runtime/middleware"
)

func main() {
	http.Handle("/", middleware.SwaggerUI(middleware.SwaggerUIOpts{Path: "/", SpecURL: "swagger.yaml"}, nil))

	http.Handle("/swagger.yaml", http.FileServer(http.Dir("./")))

	log.Println(http.ListenAndServe(":80", nil))
}
