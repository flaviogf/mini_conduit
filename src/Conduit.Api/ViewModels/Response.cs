using System.Collections.Generic;
using System.Linq;

namespace Conduit.Api.ViewModels
{
    public class Response
    {
        protected Response(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public IEnumerable<string> Errors { get; }

        public static Response Success()
        {
            return new Response(Enumerable.Empty<string>());
        }

        public static Response Failure(IEnumerable<string> errors)
        {
            return new Response(errors);
        }

        public static Response<T> Success<T>(T content)
        {
            return new Response<T>(content, Enumerable.Empty<string>());
        }
    }

    public class Response<T> : Response
    {
        public Response(T content, IEnumerable<string> errors) : base(errors)
        {
            Content = content;
        }

        public T Content { get; }
    }
}
