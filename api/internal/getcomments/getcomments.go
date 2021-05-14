package getcomments

import (
	"database/sql"
	"encoding/json"
	"log"
	"net/http"
	"time"

	"github.com/flaviogf/conduit/api/internal/models"
	"github.com/gorilla/mux"
)

type CommentsResponse struct {
	Comments []Comment `json:"comments"`
}

type Comment struct {
	ID        int64     `json:"id"`
	CreatedAt time.Time `json:"createdAt"`
	UpdatedAt time.Time `json:"updatedAt"`
	Body      string    `json:"body"`
	Author    Author    `json:"author"`
}

type Author struct {
	Username string `json:"username"`
	Bio      string `json:"bio"`
	Image    string `json:"image"`
}

func GetCommentsHandler(rw http.ResponseWriter, r *http.Request) {
	ctx, err := models.Context(r.Context())

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	vars := mux.Vars(r)

	article, err := models.GetArticle(ctx, vars["slug"])

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

	comments, err := article.GetComments(ctx)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	commentsList := make([]Comment, 0)

	for _, comment := range comments {
		author, err := comment.GetAuthor(ctx)

		if err != nil {
			log.Println(err)

			rw.WriteHeader(http.StatusInternalServerError)

			return
		}

		commentsList = append(commentsList, Comment{
			comment.ID,
			comment.CreatedAt,
			comment.UpdatedAt,
			comment.Body,
			Author{
				author.Username,
				author.Bio,
				author.Image,
			},
		})
	}

	response := CommentsResponse{commentsList}

	enc := json.NewEncoder(rw)

	enc.Encode(response)
}
