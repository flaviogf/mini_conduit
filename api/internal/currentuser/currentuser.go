package currentuser

import (
	"database/sql"
	"encoding/json"
	"log"
	"net/http"
	"os"

	"github.com/flaviogf/conduit/api/internal/models"
)

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

func CurrentUserHandler(rw http.ResponseWriter, r *http.Request) {
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

	response := UserResponse{User{
		user.Email,
		user.Token(os.Getenv("CONDUIT_SECRET")),
		user.Username,
		user.Bio,
		user.Image,
	}}

	enc := json.NewEncoder(rw)

	enc.Encode(response)
}
