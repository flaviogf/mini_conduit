using System.Collections.Generic;

namespace Conduit.Api.ViewModels
{
    public class Error
    {
        public Error(IEnumerable<string> body)
        {
            Body = body;
        }

        public IEnumerable<string> Body { get; }
    }
}
