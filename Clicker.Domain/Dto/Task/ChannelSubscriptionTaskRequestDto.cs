using System.ComponentModel.DataAnnotations;

namespace Clicker.Domain.Dto.Task;

public record ChannelSubscriptionTaskRequestDto([Required(AllowEmptyStrings = false, ErrorMessage = "Channel Id is Required!")]
     string ChannelId );