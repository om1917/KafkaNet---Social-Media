using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewPostController : ControllerBase
    {
        private readonly ILogger<NewPostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public NewPostController(ILogger<NewPostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] NewPostCommand command)
        {
            var id = Guid.NewGuid();
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);
            
                return StatusCode(StatusCodes.Status201Created, new NewPostResponse{
                    Id = id,
                    Message = "New Post created successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning,ex, "Validation failed for CreatePostCommand");
                return BadRequest(new BaseResponse
                {
                    Message = $"Validation failed: {ex.Message}"
                }); 
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,ex, "Error occurred while processing CreatePostCommand");
                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Id = id,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
            
        }
    }
}