using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOS.BasketDTOs
{
    //public class BasketDTO
    //{
    //    public string Id { get; set; } = default!;
    //    public ICollection<BasketItemDTO> Items { get; set; } = [];
    //}

    public record BasketDTO(
        string Id,
        ICollection<BasketItemDTO> Items,
        string? ClientSecret,
        string? PaymentIntentId,
        int? DeliveryMethodId,
        decimal? ShippingPrice);
}
