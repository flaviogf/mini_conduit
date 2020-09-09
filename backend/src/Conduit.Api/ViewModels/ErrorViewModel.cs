using System.Collections.Generic;

namespace Conduit.Api.ViewModels
{
    public class ErrorViewModel
    {
        public ErrorViewModel(IEnumerable<string> messages)
        {
            Body = messages;
        }

        public IEnumerable<string> Body { get; }
    }
}
