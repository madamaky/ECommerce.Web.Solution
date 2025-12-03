using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Shared.DTOS.BasketDTOs;

namespace ECommerce.Service.Abstraction
{
    public interface IPaymentService
    {
        Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string basketId);
    }
}
