using Clicker.BL.Abstractions;
using Clicker.Domain.Abstractions;
using Telegram.Bot.Types;
using User = Clicker.DAL.Models.User;

namespace Clicker.BL.Implementations;

public class EnergyRefillService: IEnergyRefillService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseEntityRepository<User> _userRepository;
    private readonly IUserService _userService;

    public EnergyRefillService(IUnitOfWork unitOfWork, IBaseEntityRepository<User> userRepository, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _userService = userService;
    }
    public async Task RefillEnergyAsync(CancellationToken cancellationToken)
    {
        var allUsers = await _userRepository.GetAllAsync(cancellationToken);
        foreach (var user in allUsers)
        {
            user.Energy = (int)await _userService.CalculateEnergyLimitToRecoverAsync(user.Id, cancellationToken);
            _userRepository.Update(user); 
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}