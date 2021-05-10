package newarticle

import (
	"encoding/json"
	"log"
	"net/http"
	"regexp"
	"strings"
	"time"

	"github.com/flaviogf/conduit/api/internal/models"
)

type NewArticleRequest struct {
	Article NewArticle
}

type NewArticle struct {
	Title       string   `json:"title"`
	Description string   `json:"description"`
	Body        string   `json:"body"`
	TagList     []string `json:"tagList"`
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

func NewArticleHandler(rw http.ResponseWriter, r *http.Request) {
	var request NewArticleRequest

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

	article := models.NewArticle(0, slugify(request.Article.Title), request.Article.Title, request.Article.Description, request.Article.Body, time.Now(), time.Now())

	err = article.Save(ctx)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		_ = ctx.Value("tx").(models.Tx).Rollback()

		return
	}

	err = article.AddTags(ctx, request.Article.TagList)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		_ = ctx.Value("tx").(models.Tx).Rollback()

		return
	}

	user, err := models.GetCurrentUser(ctx)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		_ = ctx.Value("tx").(models.Tx).Rollback()

		return
	}

	_ = ctx.Value("tx").(models.Tx).Commit()

	response := ArticleResponse{Article{
		article.Slug,
		article.Title,
		article.Description,
		article.Body,
		request.Article.TagList,
		article.CreatedAt,
		article.UpdatedAt,
		Author{
			user.Username,
			user.Bio,
			user.Image,
		},
	}}

	enc := json.NewEncoder(rw)

	enc.Encode(response)
}

func slugify(text string) string {
	return strings.ToLower(regexp.MustCompile(`\s`).ReplaceAllString(text, "-"))
}
