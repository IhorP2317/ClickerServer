using Clicker.API.Filters;
using Clicker.BL.Abstractions;
using Clicker.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clicker.API.Controllers;
[ApiController]
[Route("api/users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    [ValidateModel]
    public async Task<IActionResult> RegisterAsync([FromBody] UserSignUpDto userSignUpDto,
        CancellationToken cancellationToken) {
        var response = await userService.RegisterUserAsync(userSignUpDto, cancellationToken);

        return Ok(response);
    }
    [Authorize]
    [HttpPost("{id:guid}/click")]
    public async Task<IActionResult> ClickAsync([FromRoute] Guid id, [FromQuery] int count,
        CancellationToken cancellationToken) {
        await userService.ClickAsync(id, count, cancellationToken);

        return NoContent();
    }
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken) {
       var user =  await userService.GetUserByIdAsync(id, cancellationToken);

        return user != null ? Ok(user) : NotFound(
            new ProblemDetails{ 
                Status = StatusCodes.Status404NotFound,
                 Title = "User Not Found",
                 Detail = $"User with ID {id} was not found." 
            });
    }

    [Authorize]
    [HttpGet("{id:guid}/tasks")]
    public async Task<IActionResult> GetUserTasksAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userTasks = await userService.GetUserTasksAsync(id, cancellationToken);
        return Ok(userTasks);
    }
    [Authorize]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> AdjustUserEnergyAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
         await userService.AdjustUserEnergyAsync(id, cancellationToken);
        return NoContent();
    }
}
