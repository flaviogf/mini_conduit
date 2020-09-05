using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers
{
    public abstract class ApplicationController : ControllerBase
    {
        public IActionResult Created(object value)
        {
            var result = new ObjectResult(value)
            {
                StatusCode = 201
            };

            return result;
        }

        public IActionResult UnexpectedError(object value)
        {
            var result = new ObjectResult(value)
            {
                StatusCode = 422
            };

            return result;
        }
    }
}
