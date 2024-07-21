using Clicker.API.Filters;
using Clicker.BL.Abstractions;
using Clicker.Domain.Dto.Task;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clicker.API.Controllers;
[Authorize]
[ApiController]
[Route("api")]
public class ChannelTaskController(
    IChannelSubscriptionTaskService channelSubscriptionTaskService,
    IUserChannelTaskService userChannelTaskService):ControllerBase
{
    [HttpPost("channelTasks")]
    [ValidateModel]
    public async Task<IActionResult> CreateChannelTaskAsync(
        ChannelSubscriptionTaskRequestDto channelSubscriptionTaskRequestDto, CancellationToken cancellationToken)
    {
        await channelSubscriptionTaskService.CreateChannelSubscriptionTaskAsync(channelSubscriptionTaskRequestDto,
            cancellationToken);
        return NoContent();
    }
    [HttpGet("users/{userId:guid}/channelTasks/{taskId:guid}/status")]
    public async Task<IActionResult> CheckChannelSubscriptionAsync( [FromRoute] Guid userId, [FromRoute] Guid taskId,
        CancellationToken cancellationToken)
    {
       
        return Ok( await userChannelTaskService.IsUserSubscribedAsync(userId, taskId, cancellationToken));
    }

    [HttpPost("users/{userId:guid}/channelTasks/{taskId:guid}")]
    public async Task<IActionResult> CreateUserOfferSubscriptionTaskAsync([FromRoute] Guid userId, [FromRoute] Guid taskId, CancellationToken cancellationToken)
    {
        await userChannelTaskService.CreateUserTaskAsync(userId, taskId, cancellationToken);
        return NoContent();
    }
}