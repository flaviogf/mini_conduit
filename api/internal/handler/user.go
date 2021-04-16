package handler

import (
	"encoding/json"
	"log"
	"net/http"

	"github.com/flaviogf/conduit/api/internal/model"
	"golang.org/x/crypto/bcrypt"
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

		rw.WriteHeader(http.StatusUnprocessableEntity)

		return
	}

	user, err := model.GetUserByEmail(r.Context(), request.User.Email)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusUnauthorized)

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

type RegisterUserRequest struct {
	User RegisterUser `json:"user"`
}

type RegisterUser struct {
	Username string `json:"username"`
	Email    string `json:"email"`
	Password string `json:"password"`
}

func RegisterUserHandler(rw http.ResponseWriter, r *http.Request) {
	dec := json.NewDecoder(r.Body)

	var request RegisterUserRequest

	err := dec.Decode(&request)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusUnprocessableEntity)

		return
	}

	user, _ := model.GetUserByEmail(r.Context(), request.User.Email)

	if user != nil {
		log.Println("email already taken")

		rw.WriteHeader(http.StatusUnprocessableEntity)

		return
	}

	hash, err := bcrypt.GenerateFromPassword([]byte(request.User.Password), 8)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	user = model.NewUser(0, request.User.Username, request.User.Email, string(hash), "", "")

	err = user.Save(r.Context())

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

	rw.WriteHeader(http.StatusCreated)

	response := UserResponse{User{user.Email, token, user.Username, user.Bio, user.Image}}

	enc := json.NewEncoder(rw)

	err = enc.Encode(response)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}
}
