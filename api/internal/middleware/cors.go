package middleware

import "net/http"

func Cors(next http.Handler) http.Handler {
	return http.HandlerFunc(func(rw http.ResponseWriter, r *http.Request) {
		rw.Header().Set("Access-Control-Allow-Origin", "*")
		rw.Header().Set("Access-Control-Allow-Methods", "GET, POST, PUT, OPTIONS")
		rw.Header().Set("Access-Control-Allow-Headers", "*")

		next.ServeHTTP(rw, r)
	})
}
