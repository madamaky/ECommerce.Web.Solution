using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOS.OrderDTOs
{
    public class OrderDTO
    {
        public string BasketId { get; set; } = null!;
        public int DeliveryMethodId { get; set; }
        public AddressDTO Address { get; set; }
    }
}
