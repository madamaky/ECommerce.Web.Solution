using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.OrderDTOs;

namespace ECommerce.Service.Abstraction
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email);

        Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email);

        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods();

        Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id, string email);
    }
}
