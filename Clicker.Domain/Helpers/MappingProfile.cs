using AutoMapper;
using Clicker.DAL.Models;
using Clicker.Domain.Dto;
using Clicker.Domain.Dto.Task;

namespace Clicker.Domain.Helpers;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<UserSecurityResponseDto,User>();
        CreateMap<User, UserResponseDto>().ReverseMap();
      
        CreateMap<OfferSubscriptionTaskRequestDto, OfferSubscriptionTask>();
        CreateMap<OfferSubscriptionTask, OfferSubscriptionTaskResponseDto>();
        CreateMap<UserOfferSubscriptionTask, UserOfferSubscriptionTaskResponseDto>()
            .ForMember(dest => dest.OfferUrl, opt => opt.MapFrom(src => src.OfferTask.OfferUrl)).ReverseMap();
        
        CreateMap<ChannelSubscriptionTaskRequestDto, ChannelSubscriptionTask>();
        CreateMap<ChannelSubscriptionTask, ChannelSubscriptionTaskResponseDto>();
        CreateMap<UserChannelSubscriptionTask, UserChannelSubscriptionTaskResponseDto>()
            .ForMember(dest => dest.ChannelId, opt => opt.MapFrom(src => src.ChannelSubscriptionTask.ChannelId)).ReverseMap();
        CreateMap<ICollection<UserChannelSubscriptionTask>, ICollection<UserChannelSubscriptionTaskResponseDto>>().ConvertUsing(new CollectionTypeConverter<UserChannelSubscriptionTask, UserChannelSubscriptionTaskResponseDto>());
    }
    private class CollectionTypeConverter<TSource, TDestination> : ITypeConverter<ICollection<TSource>, ICollection<TDestination>>
    {
        public ICollection<TDestination> Convert(ICollection<TSource> source, ICollection<TDestination> destination, ResolutionContext context)
        {
            if (source == null) return null;

            destination = destination ?? new List<TDestination>();

            foreach (var item in source)
            {
                destination.Add(context.Mapper.Map<TDestination>(item));
            }

            return destination;
        }
    }
}