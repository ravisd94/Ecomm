using API.DTO;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized("This is an unauthorized response");
        }
        
        [HttpGet("badRequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("This is a bad request response");
        }

        [HttpGet("notFound")]
        public IActionResult GetNotFound()
        {
            return NotFound("This is a not found response");
        }
        [HttpGet("internalError")]
        public IActionResult GetInternalError()
        {
            throw new Exception("This is an internal server error response");
        }

        [HttpPost("validationError")]
        public IActionResult GetValidationError(CreateProductDto createProductDto)
        {
            return Ok();
        }
    }
}
