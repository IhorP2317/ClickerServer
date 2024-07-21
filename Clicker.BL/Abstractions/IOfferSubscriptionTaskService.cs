using Clicker.DAL.Models;
using Clicker.Domain.Dto.Task;

namespace Clicker.BL.Abstractions;

public interface IOfferSubscriptionTaskService
{
    Task<OfferSubscriptionTaskResponseDto> GetOfferSubscriptionTaskByIdAsync(Guid id,
        CancellationToken cancellationToken);
    Task<bool> IsSubscriptionTaskExistAsync(Guid id ,CancellationToken cancellationToken = default);
    Task<OfferSubscriptionTaskResponseDto> CreateOfferSubscriptionTaskAsync(OfferSubscriptionTaskRequestDto offerSubscriptionTaskRequestDto,
        CancellationToken cancellationToken = default);
    Task UpdateOfferSubscriptionTaskAsync(Guid id, OfferSubscriptionTaskRequestDto offerSubscriptionTaskRequestDto,
        CancellationToken cancellationToken = default);
    Task DeleteOfferSubscriptionTaskAsync(Guid id ,CancellationToken cancellationToken = default);
}