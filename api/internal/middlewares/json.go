package middlewares

import "net/http"

func Json(next http.Handler) http.Handler {
	return http.HandlerFunc(func(rw http.ResponseWriter, r *http.Request) {
		rw.Header().Set("Content-Type", "application/json")

		next.ServeHTTP(rw, r)
	})
}
