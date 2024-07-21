using Clicker.API.Filters;
using Clicker.BL.Abstractions;
using Clicker.DAL.Migrations;
using Clicker.Domain.Dto.Task;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clicker.API.Controllers;
[ApiController]
[Route("api")]
public class OfferTaskController(IOfferSubscriptionTaskService offerSubscriptionTaskService, IUserOfferTaskService userOfferTaskService)
    : ControllerBase
{
    [HttpPost("offerTasks")]
    [Authorize]
    [ValidateModel]
    public async Task<IActionResult> CreateOfferTaskAsync(
        OfferSubscriptionTaskRequestDto offerSubscriptionTaskRequestDto, CancellationToken cancellationToken)
    {
        await offerSubscriptionTaskService.CreateOfferSubscriptionTaskAsync(offerSubscriptionTaskRequestDto,
            cancellationToken);
        return NoContent();
    }
    [HttpGet("users/{userId:guid}/offerTasks/{taskId:guid}/status")]
    [Authorize]
    public async Task<IActionResult> CheckOfferSubscriptionAsync( [FromRoute] Guid userId, [FromRoute] Guid taskId,
         CancellationToken cancellationToken)
    {
       
        return Ok( await userOfferTaskService.IsUserSubscribedAsync(userId, taskId, cancellationToken));
    }

    [HttpPost("users/{userId:guid}/offerTasks/{taskId:guid}")]
    [Authorize]
    public async Task<IActionResult> CreateUserOfferSubscriptionTaskAsync([FromRoute] Guid userId, [FromRoute] Guid taskId, CancellationToken cancellationToken)
    {
        await userOfferTaskService.CreateUserTaskAsync(userId, taskId, cancellationToken);
        return NoContent();
    }
}
