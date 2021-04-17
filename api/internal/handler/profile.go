package handler

import (
	"encoding/json"
	"log"
	"net/http"

	"github.com/flaviogf/conduit/api/internal/model"
	"github.com/gorilla/mux"
)

type ProfileResponse struct {
	Profile Profile `json:"profile"`
}

type Profile struct {
	Username  string `json:"username"`
	Bio       string `json:"bio"`
	Image     string `json:"image"`
	Following bool   `json:"following"`
}

func GetProfile(rw http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)

	user, err := model.GetUserByUsername(r.Context(), vars["username"])

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusNotFound)

		return
	}

	response := ProfileResponse{Profile{user.Username, user.Bio, user.Image, true}}

	enc := json.NewEncoder(rw)

	err = enc.Encode(response)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}
}
