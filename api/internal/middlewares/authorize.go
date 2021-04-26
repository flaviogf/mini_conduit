package middlewares

import (
	"context"
	"log"
	"net/http"
	"os"
	"strconv"

	"github.com/dgrijalva/jwt-go"
)

func Authorize(next http.Handler) http.Handler {
	return http.HandlerFunc(func(rw http.ResponseWriter, r *http.Request) {
		authorization := r.Header.Get("Authorization")

		if len(authorization) < 7 {
			rw.WriteHeader(http.StatusUnauthorized)

			return
		}

		tokenString := authorization[7:]

		token, err := jwt.Parse(tokenString, func(token *jwt.Token) (interface{}, error) {
			return []byte(os.Getenv("CONDUIT_KEY")), nil
		})

		if err != nil {
			log.Println(err)

			rw.WriteHeader(http.StatusUnauthorized)

			return
		}

		claims, ok := token.Claims.(jwt.MapClaims)

		if !ok || !token.Valid {
			rw.WriteHeader(http.StatusUnauthorized)

			return
		}

		userId, err := strconv.Atoi(claims["sub"].(string))

		if err != nil {
			log.Println(err)

			rw.WriteHeader(http.StatusUnauthorized)

			return
		}

		ctx := context.WithValue(r.Context(), "userId", userId)

		next.ServeHTTP(rw, r.WithContext(ctx))
	})
}
