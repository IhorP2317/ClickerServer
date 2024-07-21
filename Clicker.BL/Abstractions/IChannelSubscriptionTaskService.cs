using System.Threading.Channels;
using Clicker.Domain.Dto.Task;

namespace Clicker.BL.Abstractions;

public interface IChannelSubscriptionTaskService
{
    Task<ChannelSubscriptionTaskResponseDto> GetChannelSubscriptionTaskByIdAsync(Guid id,
        CancellationToken cancellationToken);
    Task<bool> IsSubscriptionTaskExistAsync(Guid id ,CancellationToken cancellationToken = default);
    Task<ChannelSubscriptionTaskResponseDto> CreateChannelSubscriptionTaskAsync(
        ChannelSubscriptionTaskRequestDto channelSubscriptionTaskRequestDto,
        CancellationToken cancellationToken = default);
    Task UpdateChannelSubscriptionTaskAsync(Guid id,
        ChannelSubscriptionTaskRequestDto channelSubscriptionTaskRequestDto,
        CancellationToken cancellationToken = default);
    Task DeleteChannelSubscriptionTaskAsync(Guid id ,CancellationToken cancellationToken = default);
}