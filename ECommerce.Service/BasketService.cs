using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Service.Abstraction;
using ECommerce.Service.Exceptions;
using ECommerce.Shared.DTOS.BasketDTOs;

namespace ECommerce.Service
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket)
        {
            var CustomerBasket = _mapper.Map<CustomerBasket>(basket);
            var createdOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(CustomerBasket);
            return _mapper.Map<BasketDTO>(createdOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string id)
            => await _basketRepository.DeleteBasketAsync(id);

        public async Task<BasketDTO> GetBasketAsync(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket is null)
                throw new BasketNotFoundException(id);
            return _mapper.Map<BasketDTO>(basket);
        }
    }
}
