package updatearticle

import (
	"database/sql"
	"encoding/json"
	"log"
	"net/http"
	"regexp"
	"strings"
	"time"

	"github.com/flaviogf/conduit/api/internal/models"
	"github.com/gorilla/mux"
)

type UpdateArticleRequest struct {
	Article UpdateArticle `json:"article"`
}

type UpdateArticle struct {
	Title       string `json:"title"`
	Description string `json:"description"`
	Body        string `json:"body"`
}

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

func UpdateArticleHandler(rw http.ResponseWriter, r *http.Request) {
	var request UpdateArticleRequest

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
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	newArticle := models.NewArticle(article.ID, slugify(request.Article.Title), request.Article.Title, request.Article.Description, request.Article.Body, article.CreatedAt, time.Now())

	err = newArticle.Save(ctx)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		_ = ctx.Value("tx").(models.Tx).Rollback()

		return
	}

	author, err := article.GetAuthor(ctx)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		_ = ctx.Value("tx").(models.Tx).Rollback()

		return
	}

	tags, err := article.GetTags(ctx)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		_ = ctx.Value("tx").(models.Tx).Rollback()

		return
	}

	_ = ctx.Value("tx").(models.Tx).Commit()

	response := ArticleResponse{Article{
		newArticle.Slug,
		newArticle.Title,
		newArticle.Description,
		newArticle.Body,
		tags,
		newArticle.CreatedAt,
		newArticle.UpdatedAt,
		Author{
			author.Username,
			author.Bio,
			author.Image,
		},
	}}

	enc := json.NewEncoder(rw)

	enc.Encode(response)
}

func slugify(text string) string {
	return strings.ToLower(regexp.MustCompile(`\s`).ReplaceAllString(text, "-"))
}
