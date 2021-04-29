package updateuser

import (
	"database/sql"
	"encoding/json"
	"log"
	"net/http"
	"os"

	"github.com/flaviogf/conduit/api/internal/models"
	"golang.org/x/crypto/bcrypt"
)

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

func UpdateUserHandler(rw http.ResponseWriter, r *http.Request) {
	var request UpdateUserRequest

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

	user, err := models.GetCurrentUser(ctx)

	if err != nil && err == sql.ErrNoRows {
		log.Println(err)

		rw.WriteHeader(http.StatusNotFound)

		return
	}

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	passwordHash, err := bcrypt.GenerateFromPassword([]byte(request.User.Password), 8)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	newUser := models.NewUser(user.ID, request.User.Username, request.User.Email, string(passwordHash), request.User.Bio, request.User.Image)

	err = newUser.Save(ctx)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		_ = ctx.Value("tx").(models.Tx).Rollback()

		return
	}

	_ = ctx.Value("tx").(models.Tx).Commit()

	response := UserResponse{User{
		newUser.Email,
		newUser.Token(os.Getenv("CONDUTI_KEY")),
		newUser.Username,
		newUser.Bio,
		newUser.Image,
	}}

	enc := json.NewEncoder(rw)

	enc.Encode(response)
}
