package middleware

import (
	"log"
	"net/http"
	"strconv"
)

func Authorize(next http.Handler) http.Handler {
	return http.HandlerFunc(func(rw http.ResponseWriter, r *http.Request) {
		_, err := strconv.ParseInt(r.Header.Get("X-User"), 10, 64)

		if err != nil {
			log.Println(err)

			rw.WriteHeader(http.StatusUnauthorized)

			return
		}

		next.ServeHTTP(rw, r)
	})
}
