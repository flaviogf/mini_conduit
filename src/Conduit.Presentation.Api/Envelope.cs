using System.Collections.Generic;
using System.Linq;

namespace Conduit.Presentation.Api
{
    public class Envelope
    {
        protected Envelope(bool isSuccess, IEnumerable<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public bool IsSuccess { get; }

        public IEnumerable<string> Errors { get; }

        public static Envelope Success()
        {
            return new Envelope(true, Enumerable.Empty<string>());
        }

        public static Envelope Failure(IEnumerable<string> errors)
        {
            return new Envelope(false, errors);
        }

        public static Envelope Failure(params string[] errors)
        {
            return new Envelope(false, errors);
        }

        public static Envelope<T> Success<T>(T content)
        {
            return new Envelope<T>(content, true, Enumerable.Empty<string>());
        }

        public static Envelope<T> Failure<T>(IEnumerable<string> errors)
        {
            return new Envelope<T>(default, false, errors);
        }

        public static Envelope<T> Failure<T>(params string[] errors)
        {
            return new Envelope<T>(default, false, errors);
        }
    }

    public class Envelope<T> : Envelope
    {
        public Envelope(T content, bool isSuccess, IEnumerable<string> errors) : base(isSuccess, errors)
        {
            Content = content;
        }

        public T Content { get; }
    }
}
