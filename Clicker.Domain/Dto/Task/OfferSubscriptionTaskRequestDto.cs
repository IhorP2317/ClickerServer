using System.ComponentModel.DataAnnotations;

namespace Clicker.Domain.Dto.Task;

public record OfferSubscriptionTaskRequestDto
(
    [property:Required(AllowEmptyStrings = false, ErrorMessage = "Offer URL is Required!")]
    [property:Url] string OfferUrl 
);