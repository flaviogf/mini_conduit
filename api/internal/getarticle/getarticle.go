package getarticle

import (
	"database/sql"
	"encoding/json"
	"log"
	"net/http"
	"time"

	"github.com/flaviogf/conduit/api/internal/models"
	"github.com/gorilla/mux"
)

type ArticleResponse struct {
	Article Article `json:"article"`
}

type Article struct {
	Slug        string    `json:"slug"`
	Title       string    `json:"title"`
	Description string    `json:"description"`
	Body        string    `json:"body"`
	TagList     []string  `json:"tagList"`
	CreatedAt   time.Time `json:"createdAt"`
	UpdatedAt   time.Time `json:"updatedAt"`
	Author      Author    `json:"author"`
}

type Author struct {
	Username string `json:"username"`
	Bio      string `json:"bio"`
	Image    string `json:"image"`
}

func GetArticleHandler(rw http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)

	ctx, err := models.Context(r.Context())

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

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

	author, err := article.GetAuthor(ctx)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	tags, err := article.GetTags(ctx)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	response := ArticleResponse{Article{
		article.Slug,
		article.Title,
		article.Description,
		article.Body,
		tags,
		article.CreatedAt,
		article.UpdatedAt,
		Author{
			author.Username,
			author.Bio,
			author.Image,
		},
	}}

	enc := json.NewEncoder(rw)

	enc.Encode(response)
}
