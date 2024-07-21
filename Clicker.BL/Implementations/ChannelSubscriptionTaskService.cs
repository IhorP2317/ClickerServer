using AutoMapper;
using Clicker.BL.Abstractions;
using Clicker.DAL.Models;
using Clicker.Domain.Abstractions;
using Clicker.Domain.Dto.Task;
using Clicker.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Clicker.BL.Implementations;

public class ChannelSubscriptionTaskService:IChannelSubscriptionTaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseEntityRepository<ChannelSubscriptionTask> _taskRepository;
    private readonly IMapper _mapper;

    public ChannelSubscriptionTaskService(IUnitOfWork unitOfWork,
        IBaseEntityRepository<ChannelSubscriptionTask> taskRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _taskRepository = taskRepository;
        _mapper = mapper;
    }
    
    public async Task<ChannelSubscriptionTaskResponseDto> GetChannelSubscriptionTaskByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
        if (task == null)
            throw new ApiException($"Channel task with id {id} is not found!", StatusCodes.Status404NotFound);
        return _mapper.Map<ChannelSubscriptionTaskResponseDto>(task);
    }

    public async Task<bool> IsSubscriptionTaskExistAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _taskRepository.IsExistAsync(id, cancellationToken);
 

    public async  Task<ChannelSubscriptionTaskResponseDto> CreateChannelSubscriptionTaskAsync(ChannelSubscriptionTaskRequestDto channelSubscriptionTaskRequestDto,
        CancellationToken cancellationToken = default)
    {
        var addedTask =    await _taskRepository.AddAsync(_mapper.Map<ChannelSubscriptionTask>(channelSubscriptionTaskRequestDto),
            cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
     return _mapper.Map<ChannelSubscriptionTaskResponseDto>(addedTask);
    }

    public async  Task UpdateChannelSubscriptionTaskAsync(Guid id, ChannelSubscriptionTaskRequestDto channelSubscriptionTaskRequestDto,
        CancellationToken cancellationToken = default)
    {
        if (await IsSubscriptionTaskExistAsync(id, cancellationToken))
            throw new ApiException(
                $"Task with channel id {channelSubscriptionTaskRequestDto.ChannelId} is not exist!",
                StatusCodes.Status404NotFound);
        _taskRepository.Update(_mapper.Map<ChannelSubscriptionTask>(channelSubscriptionTaskRequestDto));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

    }
  
    public async Task DeleteChannelSubscriptionTaskAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var task =  await _taskRepository.GetByIdAsync(id, cancellationToken);
        if (task == null)
            throw new ApiException(
                $"Task with channel id {id} is not exist!",
                StatusCodes.Status404NotFound);
        _taskRepository.Delete(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}