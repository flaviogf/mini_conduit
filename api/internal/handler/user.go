package handler

import (
	"encoding/json"
	"log"
	"net/http"
	"strconv"

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

	token, err := user.Token()

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

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

func GetCurrentUserHandler(rw http.ResponseWriter, r *http.Request) {
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

	token, err := user.Token()

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

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

type UpdateUserRequest struct {
	User UpdateUser `json:"user"`
}

type UpdateUser struct {
	Email    string `json:"email"`
	Username string `json:"username"`
	Password string `json:"password"`
	Image    string `json:"image"`
	Bio      string `json:"bio"`
}

func UpdateUserHandler(rw http.ResponseWriter, r *http.Request) {
	dec := json.NewDecoder(r.Body)

	var request UpdateUserRequest

	err := dec.Decode(&request)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusUnprocessableEntity)

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

	sameEmail, err := model.GetUserByEmail(r.Context(), request.User.Email)

	if sameEmail != nil && user.ID != sameEmail.ID {
		log.Println("email already taken")

		rw.WriteHeader(http.StatusUnprocessableEntity)

		return
	}

	hash, err := getPasswordHash(request, user)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	user = model.NewUser(user.ID, request.User.Username, request.User.Email, string(hash), request.User.Bio, request.User.Image)

	err = user.Save(r.Context())

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	token, err := user.Token()

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

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

func getPasswordHash(request UpdateUserRequest, user *model.User) ([]byte, error) {
	if len(request.User.Password) > 0 {
		return bcrypt.GenerateFromPassword([]byte(request.User.Password), 8)
	}

	return []byte(user.PasswordHash), nil
}
