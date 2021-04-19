package handler

import (
	"encoding/json"
	"log"
	"net/http"

	"github.com/flaviogf/conduit/api/internal/model"
)

type TagResponse struct {
	Tags []string `json:"tags"`
}

func GetTagsHandler(rw http.ResponseWriter, r *http.Request) {
	tags, err := model.GetTags(r.Context())

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	response := TagResponse{}

	for _, tag := range tags {
		response.Tags = append(response.Tags, tag.Content)
	}

	enc := json.NewEncoder(rw)

	err = enc.Encode(response)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}
}
