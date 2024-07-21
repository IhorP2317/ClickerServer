using Clicker.Domain.Dto;
using Clicker.Domain.Dto.Task;
using Clicker.Domain.Enums;

namespace Clicker.BL.Abstractions;

public interface IUserService
{
    Task<UserResponseDto> RegisterUserAsync(UserSignUpDto userSignUpDto, CancellationToken cancellationToken = default);
    Task<UserResponseDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsUserExistAsync(Guid id, CancellationToken cancellationToken = default);
    Task ClickAsync(Guid userId, int clickCount = 1, CancellationToken cancellationToken = default);
    Task<UserTasksResponseDto> GetUserTasksAsync(Guid id, CancellationToken cancellationToken = default);
    Task<EnergyLimit> CalculateEnergyLimitToRecoverAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AdjustUserEnergyAsync(Guid userId, CancellationToken cancellationToken = default);

}
   
