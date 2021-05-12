package addcomment

import (
	"database/sql"
	"encoding/json"
	"log"
	"net/http"
	"time"

	"github.com/flaviogf/conduit/api/internal/models"
	"github.com/gorilla/mux"
)

type AddCommentRequest struct {
	Comment AddComment `json:"comment"`
}

type AddComment struct {
	Body string `json:"body"`
}

type CommentResponse struct {
	Comment Comment `json:"comment"`
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

func AddCommentHandler(rw http.ResponseWriter, r *http.Request) {
	var request AddCommentRequest

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

	vars := mux.Vars(r)

	article, err := models.GetArticle(ctx, vars["slug"])

	if err != nil && err == sql.ErrNoRows {
		log.Println(err)

		rw.WriteHeader(http.StatusNotFound)

		return
	}

	if err != nil {
		log.Print(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	comment := models.NewComment(0, request.Comment.Body, time.Now(), time.Now())

	err = article.AddComment(ctx, comment)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		_ = ctx.Value("tx").(models.Tx).Rollback()

		return
	}

	author, err := models.GetCurrentUser(ctx)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		_ = ctx.Value("tx").(models.Tx).Rollback()

		return
	}

	_ = ctx.Value("tx").(models.Tx).Commit()

	response := CommentResponse{Comment{
		comment.ID,
		comment.CreatedAt,
		comment.UpdatedAt,
		comment.Body,
		Author{
			author.Username,
			author.Bio,
			author.Image,
		},
	}}

	rw.WriteHeader(http.StatusCreated)

	enc := json.NewEncoder(rw)

	enc.Encode(response)
}
