package newuser

import (
	"encoding/json"
	"log"
	"net/http"
	"os"

	"github.com/flaviogf/conduit/api/internal/models"
	"golang.org/x/crypto/bcrypt"
)

type NewUserRequest struct {
	User NewUser `json:"user"`
}

type NewUser struct {
	Username string `json:"username"`
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

func NewUserHandler(rw http.ResponseWriter, r *http.Request) {
	var body NewUserRequest

	dec := json.NewDecoder(r.Body)

	err := dec.Decode(&body)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusUnprocessableEntity)

		return
	}

	passwordHash, err := bcrypt.GenerateFromPassword([]byte(body.User.Password), 8)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	ctx, err := models.Context(r.Context())

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	user := models.NewUser(0, body.User.Username, body.User.Email, string(passwordHash), "", "")

	err = user.Save(ctx)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		_ = ctx.Value("tx").(models.Tx).Rollback()

		return
	}

	_ = ctx.Value("tx").(models.Tx).Commit()

	response := UserResponse{User{
		user.Email,
		user.Token(os.Getenv("CONDUIT_KEY")),
		user.Username,
		user.Bio,
		user.Image,
	}}

	rw.WriteHeader(http.StatusCreated)

	enc := json.NewEncoder(rw)

	enc.Encode(response)
}
