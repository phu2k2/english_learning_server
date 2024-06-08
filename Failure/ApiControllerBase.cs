using english_learning_server.Failure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Extensions
{
    public abstract class ApiControllerBase : ControllerBase
    {
        [NonAction]
        public IActionResult BadRequestResponse(string message) => BadRequest(
            new ApiErrorResponse
            {
                Message = message,
            }
        );

        [NonAction]
        public IActionResult NotFoundResponse(string message) => NotFound(
            new ApiErrorResponse
            {
                Message = message,
            }
        );

        [NonAction]
        public IActionResult UnauthorizedResponse(string message) => Unauthorized(
            new ApiErrorResponse
            {
                Message = message,
            }
        );

        [NonAction]
        public IActionResult InternalServerErrorResponse(string message) => StatusCode(500,
            new ApiErrorResponse
            {
                Message = message,
            }
        );

        [NonAction]
        public IActionResult InternalServerErrorResponse(IEnumerable<IdentityError> error) => StatusCode(500, error);
    }
}