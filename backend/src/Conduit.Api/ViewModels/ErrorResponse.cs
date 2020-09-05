namespace Conduit.Api.ViewModels
{
    public class ErrorResponse
    {
        public ErrorResponse(params string[] messages)
        {
            Errors = new Error(messages);
        }

        public Error Errors { get; }
    }
}
