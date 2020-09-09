namespace Conduit.Api.ViewModels
{
    public class ErrorResponseViewModel
    {
        public ErrorResponseViewModel(params string[] errors)
        {
            Errors = new ErrorViewModel(errors);
        }

        public ErrorViewModel Errors { get; }
    }
}
