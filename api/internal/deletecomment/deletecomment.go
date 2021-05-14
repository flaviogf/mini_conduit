package deletecomment

import (
	"database/sql"
	"log"
	"net/http"
	"strconv"

	"github.com/flaviogf/conduit/api/internal/models"
	"github.com/gorilla/mux"
)

func DeleteCommentHandler(rw http.ResponseWriter, r *http.Request) {
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

	id, err := strconv.ParseInt(vars["id"], 10, 64)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusUnprocessableEntity)

		return
	}

	comment, err := article.GetComment(ctx, id)

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

	err = comment.Destroy(ctx)

	if err != nil {
		log.Println(err)

		rw.WriteHeader(http.StatusInternalServerError)

		_ = ctx.Value("tx").(models.Tx).Rollback()
	}

	_ = ctx.Value("tx").(models.Tx).Commit()

	rw.WriteHeader(http.StatusOK)
}
