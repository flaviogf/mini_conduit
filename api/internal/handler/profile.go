package handler

import (
	"encoding/json"
	"log"
	"net/http"
	"strconv"

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

func GetProfileHandler(rw http.ResponseWriter, r *http.Request) {
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

func FollowUserHandler(rw http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)

	target, err := model.GetUserByUsername(r.Context(), vars["username"])

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusNotFound)

		return
	}

	userId, err := strconv.ParseInt(r.Header.Get("X-User"), 10, 64)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusNotFound)

		return
	}

	user, err := model.GetUser(r.Context(), userId)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusNotFound)

		return
	}

	err = user.Follow(r.Context(), target)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	response := ProfileResponse{Profile{target.Username, target.Bio, target.Image, true}}

	enc := json.NewEncoder(rw)

	err = enc.Encode(response)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}
}
