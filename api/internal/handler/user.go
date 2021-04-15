package handler

import (
	"encoding/json"
	"log"
	"net/http"

	"github.com/flaviogf/conduit/api/internal/model"
)

type AuthenticateUserRequest struct {
	User AuthenticateUser `json:"user"`
}

type AuthenticateUser struct {
	Email    string `json:"email"`
	Password string `json:"password"`
}

type UserResponse struct {
	User User `json:"user"`
}

type User struct {
	Email    string `json:"email"`
	Token    string `json:"token"`
	Username string `json:"username"`
	Bio      string `json:"bio"`
	Image    string `json:"image"`
}

func AuthenticateUserHandler(rw http.ResponseWriter, r *http.Request) {
	dec := json.NewDecoder(r.Body)

	var request AuthenticateUserRequest

	err := dec.Decode(&request)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusBadRequest)

		return
	}

	user, err := model.GetUserByEmail(r.Context(), request.User.Email)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	token, err := user.Attempt(request.User.Password)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusUnauthorized)

		return
	}

	response := UserResponse{User{user.Email, token, user.Username, user.Bio, user.Image}}

	enc := json.NewEncoder(rw)

	err = enc.Encode(response)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}
}
