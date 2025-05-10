using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.HandlingErrors;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket == null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateBasket(CustomerBasketDto customerBasket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(customerBasket);

            var basket = await _basketRepository.UpdateBasketAsync(mappedBasket);
            if (basket == false)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            return Ok();
        }


        [HttpDelete]
        public async Task<ActionResult<CustomerBasket>> DeleteBasket(string id)
        {
            var basket = await _basketRepository.DeleteBasketAsync(id);
            if (basket == false)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            return Ok();
        }
    }
}
