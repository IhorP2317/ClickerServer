using AutoMapper;
using Clicker.BL.Abstractions;
using Clicker.DAL.Models;
using Clicker.Domain.Abstractions;
using Clicker.Domain.Dto.Task;
using Clicker.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Clicker.BL.Implementations;

public class OfferSubscriptionTaskService: IOfferSubscriptionTaskService
{
     private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseEntityRepository<OfferSubscriptionTask> _taskRepository;
    private readonly IMapper _mapper;

    public OfferSubscriptionTaskService(IUnitOfWork unitOfWork,
        IBaseEntityRepository<OfferSubscriptionTask> taskRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _taskRepository = taskRepository;
        _mapper = mapper;
    }
    
    public async Task<OfferSubscriptionTaskResponseDto> GetOfferSubscriptionTaskByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
        if (task == null)
            throw new ApiException($"offer task with id {id} is not found!", StatusCodes.Status404NotFound);
        return _mapper.Map<OfferSubscriptionTaskResponseDto>(task);
    }

    public async Task<bool> IsSubscriptionTaskExistAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _taskRepository.IsExistAsync(id, cancellationToken);
 

    public async  Task<OfferSubscriptionTaskResponseDto> CreateOfferSubscriptionTaskAsync(OfferSubscriptionTaskRequestDto offerSubscriptionTaskRequestDto,
        CancellationToken cancellationToken = default)
    {
        var addedTask =    await _taskRepository.AddAsync(_mapper.Map<OfferSubscriptionTask>(offerSubscriptionTaskRequestDto),
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
     return _mapper.Map<OfferSubscriptionTaskResponseDto>(addedTask);
    }

    public async  Task UpdateOfferSubscriptionTaskAsync(Guid id, OfferSubscriptionTaskRequestDto offerSubscriptionTaskRequestDto,
        CancellationToken cancellationToken = default)
    {
        if (await IsSubscriptionTaskExistAsync(id, cancellationToken))
            throw new ApiException(
                $"Task with offer id {id} is not exist!",
                StatusCodes.Status404NotFound);
        _taskRepository.Update(_mapper.Map<OfferSubscriptionTask>(offerSubscriptionTaskRequestDto));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

    }
  
    public async Task DeleteOfferSubscriptionTaskAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var task =  await _taskRepository.GetByIdAsync(id, cancellationToken);
        if (task == null)
            throw new ApiException(
                $"Task with offer id {id} is not exist!",
                StatusCodes.Status404NotFound);
        _taskRepository.Delete(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}