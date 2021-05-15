package gettags

import (
	"encoding/json"
	"log"
	"net/http"

	"github.com/flaviogf/conduit/api/internal/models"
)

type TagsResponse struct {
	Tags []string `json:"tags"`
}

func GetTagsHandler(rw http.ResponseWriter, r *http.Request) {
	ctx, err := models.Context(r.Context())

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	tags, err := models.GetTags(ctx)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	response := TagsResponse{tags}

	enc := json.NewEncoder(rw)

	enc.Encode(response)
}
