package deletearticle

import (
	"database/sql"
	"log"
	"net/http"

	"github.com/flaviogf/conduit/api/internal/models"
	"github.com/gorilla/mux"
)

func DeleteArticleHandler(rw http.ResponseWriter, r *http.Request) {
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

	err = article.Destroy(ctx)

	if err != nil {
		log.Print(err)

		rw.WriteHeader(http.StatusInternalServerError)

		_ = ctx.Value("tx").(models.Tx).Rollback()

		return
	}

	_ = ctx.Value("tx").(models.Tx).Commit()

	rw.WriteHeader(http.StatusOK)
}
