using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Service.Abstraction;
using ECommerce.Shared.DTOS.BasketDTOs;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Persentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        #region Get Basket By Id

        // GET : BaseUrl/api/basket/
        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket(string id)
        {
            var Basket = await _basketService.GetBasketAsync(id);
            return Ok(Basket);
        }

        #endregion

        #region Create Or Update Basket

        // POST : BaseUrl/api/basket/
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basket)
        {
            var Basket = await _basketService.CreateOrUpdateBasketAsync(basket);
            return Ok(Basket);
        }

        #endregion

        #region Delete Basket By Id

        // DELETE : BaseUrl/api/basket/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            var Result = await _basketService.DeleteBasketAsync(id);
            return Ok(Result);
        }

        #endregion
    }
}
