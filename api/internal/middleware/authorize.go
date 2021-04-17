package middleware

import (
	"log"
	"net/http"
	"os"

	"github.com/dgrijalva/jwt-go"
)

func Authorize(next http.Handler) http.Handler {
	return http.HandlerFunc(func(rw http.ResponseWriter, r *http.Request) {
		authorization := r.Header.Get("Authorization")

		if len(authorization) < 7 {
			log.Println("length of authorization header less than 7")

			rw.WriteHeader(http.StatusUnauthorized)

			return
		}

		tokenString := r.Header.Get("Authorization")[7:]

		token, err := jwt.Parse(tokenString, func(t *jwt.Token) (interface{}, error) {
			return []byte(os.Getenv("CONDUIT_KEY")), nil
		})

		if err != nil {
			log.Println(err)

			rw.WriteHeader(http.StatusUnauthorized)

			return
		}

		if !token.Valid {
			log.Println("token is not valid")

			rw.WriteHeader(http.StatusUnauthorized)

			return
		}

		claims, ok := token.Claims.(jwt.MapClaims)

		if !ok {
			log.Println("could not parse the claims")

			rw.WriteHeader(http.StatusUnauthorized)

			return
		}

		userId, ok := claims["sub"].(string)

		if !ok {
			log.Println("could not get the subject")

			rw.WriteHeader(http.StatusUnauthorized)

			return
		}

		r.Header.Add("X-User", userId)

		next.ServeHTTP(rw, r)
	})
}
