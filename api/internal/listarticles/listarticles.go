package listarticles

import (
	"encoding/json"
	"log"
	"net/http"
	"strconv"
	"time"

	"github.com/flaviogf/conduit/api/internal/models"
)

type ArticlesResponse struct {
	Articles []Article `json:"articles"`
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

func ListArticlesHandler(rw http.ResponseWriter, r *http.Request) {
	ctx, err := models.Context(r.Context())

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	vars := r.URL.Query()

	author := vars.Get("author")

	tag := vars.Get("tag")

	limit, _ := strconv.Atoi(vars.Get("limit"))

	offset, _ := strconv.Atoi(vars.Get("offset"))

	articles, err := models.GetArticles(ctx, author, tag, limit, offset)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	articlesList := make([]Article, len(articles))

	for index, value := range articles {
		tags, err := value.GetTags(ctx)

		if err != nil {
			log.Println(err)

			rw.WriteHeader(http.StatusInternalServerError)

			return
		}

		author, err := value.GetAuthor(ctx)

		if err != nil {
			log.Println(err)

			rw.WriteHeader(http.StatusInternalServerError)

			return
		}

		articlesList[index] = Article{
			value.Slug,
			value.Title,
			value.Description,
			value.Body,
			tags,
			value.CreatedAt,
			value.UpdatedAt,
			Author{
				author.Username,
				author.Bio,
				author.Image,
			},
		}
	}

	response := ArticlesResponse{articlesList}

	enc := json.NewEncoder(rw)

	enc.Encode(response)
}
