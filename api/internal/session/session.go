package session

import (
	"database/sql"
	"encoding/json"
	"log"
	"net/http"
	"os"

	"github.com/flaviogf/conduit/api/internal/models"
	"golang.org/x/crypto/bcrypt"
)

type SessionRequest struct {
	User Session `json:"user"`
}

type Session struct {
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

func SessionHandler(rw http.ResponseWriter, r *http.Request) {
	var request SessionRequest

	dec := json.NewDecoder(r.Body)

	err := dec.Decode(&request)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusUnprocessableEntity)

		return
	}

	ctx, err := models.Context(r.Context())

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	user, err := models.GetUserByEmail(ctx, request.User.Email)

	if err != nil && err == sql.ErrNoRows {
		log.Println(err)

		rw.WriteHeader(http.StatusUnauthorized)

		return
	}

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	err = bcrypt.CompareHashAndPassword([]byte(user.PasswordHash), []byte(request.User.Password))

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusUnauthorized)

		return
	}

	response := UserResponse{User{
		user.Email,
		user.Token(os.Getenv("CONDUIT_KEY")),
		user.Username,
		user.Bio,
		user.Image,
	}}

	enc := json.NewEncoder(rw)

	enc.Encode(response)
}
