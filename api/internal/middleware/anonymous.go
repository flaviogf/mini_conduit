package middleware

import (
	"log"
	"net/http"
	"os"

	"github.com/dgrijalva/jwt-go"
)

func Anonymous(next http.Handler) http.Handler {
	return http.HandlerFunc(func(rw http.ResponseWriter, r *http.Request) {
		r.Header.Set("X-User", "")

		authorization := r.Header.Get("Authorization")

		if len(authorization) < 7 {
			log.Println("length of authorization header less than 7")

			next.ServeHTTP(rw, r)

			return
		}

		tokenString := authorization[7:]

		token, err := jwt.Parse(tokenString, func(t *jwt.Token) (interface{}, error) {
			return []byte(os.Getenv("CONDUIT_KEY")), nil
		})

		if err != nil {
			log.Println(err)

			next.ServeHTTP(rw, r)

			return
		}

		if !token.Valid {
			log.Println("token is not valid")

			next.ServeHTTP(rw, r)

			return
		}

		claims, ok := token.Claims.(jwt.MapClaims)

		if !ok {
			log.Println("could not parse the claims")

			next.ServeHTTP(rw, r)

			return
		}

		userId, ok := claims["sub"].(string)

		if !ok {
			log.Println("could not get the subject")

			next.ServeHTTP(rw, r)

			return
		}

		r.Header.Set("X-User", userId)

		next.ServeHTTP(rw, r)
	})
}
