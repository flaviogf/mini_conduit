package handler

import (
	"encoding/json"
	"log"
	"net/http"
	"regexp"
	"strconv"
	"strings"
	"time"

	"github.com/flaviogf/conduit/api/internal/model"
	"github.com/gorilla/mux"
)

type NewArticleRequest struct {
	Article NewArticle `json:"article"`
}

type NewArticle struct {
	Title       string   `json:"title"`
	Description string   `json:"description"`
	Body        string   `json:"body"`
	Tags        []string `json:"tagList"`
}

type ArticleResponse struct {
	Article Article `json:"article"`
}

type Article struct {
	Slug           string     `json:"slug"`
	Title          string     `json:"title"`
	Description    string     `json:"description"`
	Body           string     `json:"body"`
	Tags           []string   `json:"tagList"`
	CreatedAt      *time.Time `json:"createdAt"`
	UpdatedAt      *time.Time `json:"updatedAt"`
	Favorited      bool       `json:"favorited"`
	FavoritesCount int        `json:"favoritesCount"`
	Author         Profile    `json:"author"`
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

	slug := slug(request.Article.Title)

	article, _ := model.GetArticle(r.Context(), slug)

	if article != nil {
		log.Println("slug already taken")

		rw.WriteHeader(http.StatusUnprocessableEntity)

		return
	}

	userId, err := strconv.ParseInt(r.Header.Get("X-User"), 10, 64)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusNotFound)

		return
	}

	user, err := model.GetUser(r.Context(), userId)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusNotFound)

		return
	}

	var tags []*model.Tag

	for _, content := range request.Article.Tags {
		tag := model.NewTag(0, content)

		err := tag.Save(r.Context())

		if err != nil {
			log.Println(err)

			rw.WriteHeader(http.StatusInternalServerError)

			return
		}

		tags = append(tags, tag)
	}

	now := time.Now()

	article = model.NewArticle(
		slug,
		request.Article.Title,
		request.Article.Description,
		request.Article.Body,
		&now,
		&now,
		user,
		tags,
	)

	err = article.Save(r.Context())

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}

	author := Profile{
		article.Author.Username,
		article.Author.Bio,
		article.Author.Image,
		false,
	}

	response := ArticleResponse{Article{
		article.Slug,
		article.Title,
		article.Description,
		article.Body,
		request.Article.Tags,
		article.CreatedAt,
		article.UpdatedAt,
		false,
		0,
		author,
	}}

	rw.WriteHeader(http.StatusCreated)

	enc := json.NewEncoder(rw)

	err = enc.Encode(response)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}
}

func GetArticleHandler(rw http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)

	article, err := model.GetArticle(r.Context(), vars["slug"])

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusNotFound)

		return
	}

	var tags = make([]string, 0)

	for _, tag := range article.Tags {
		tags = append(tags, tag.Content)
	}

	author := Profile{
		Username:  article.Author.Username,
		Bio:       article.Author.Bio,
		Image:     article.Author.Image,
		Following: false,
	}

	response := ArticleResponse{Article{
		Slug:           article.Slug,
		Title:          article.Title,
		Description:    article.Description,
		Body:           article.Body,
		Tags:           tags,
		CreatedAt:      article.CreatedAt,
		UpdatedAt:      article.UpdatedAt,
		Favorited:      false,
		FavoritesCount: 0,
		Author:         author,
	}}

	enc := json.NewEncoder(rw)

	err = enc.Encode(response)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}
}

func DeleteArticleHandler(rw http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)

	article, err := model.GetArticle(r.Context(), vars["slug"])

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusNotFound)

		return
	}

	err = article.Delete(r.Context())

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		return
	}
}

func slug(title string) string {
	result := strings.ToLower(title)

	r := regexp.MustCompile(`\s+`)

	result = r.ReplaceAllString(result, "-")

	return result
}
