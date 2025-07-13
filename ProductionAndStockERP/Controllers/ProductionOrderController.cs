using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Dtos.ProductionOrder;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Security.Claims;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Route("api/productionorders")]
    public class ProductionOrderController : ControllerBase
    {
        private readonly IProductionOrderService _productionOrderService;
        private readonly IMapper _mapper;

        public ProductionOrderController(IProductionOrderService productionOrderService, IMapper mapper)
        {
            _productionOrderService = productionOrderService;
            _mapper = mapper;
        }

        [HttpGet("GetAllProductionOrders")]
        public async Task<IActionResult> GetAllProductionOrders()
        {
            var result = await _productionOrderService.GetAllPrdouctionOrderAsync();
            return Ok(result);
        }

        [HttpGet("getproductionorderbyid/{id}")]
        public async Task<IActionResult> GetProductionOrderById(int id)
        {
            var result = await _productionOrderService.GetPrdouctionOrderByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("createproductionorder")]
        public async Task<IActionResult> CreateProductionOrder([FromBody] ProductionOrderCreateDto newdata)
        {
            var productionOrder=  _mapper.Map<ProductionOrder>(newdata);

            productionOrder.Status= Status.Started;
            productionOrder.CreatedAt= DateTime.Now;

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out int userId))
            {
                productionOrder.CreatedBy = userId;
            }
            else
            {
                return BadRequest("Kullanıcı kimliği token'da bulunamadı.");
            }
            var result =await _productionOrderService.CreatePrdouctionOrderAsync(productionOrder);
            return Ok(result);
        }

        [HttpPost("updateproductionorder/{id}")]
        public async Task<IActionResult> UpdateProductionOrder(int id,[FromBody] ProductionOrderUpdateDto newdata)
        {
            var productionorder = await _productionOrderService.GetPrdouctionOrderByIdAsync(id);
            if(productionorder.Data == null) {return Ok(productionorder);}

            productionorder.Data=_mapper.Map(newdata, productionorder.Data);
            var result = await _productionOrderService.UpdatePrdouctionOrderAsync(productionorder.Data);
            return Ok(result);
        }

        [HttpPost("deleteproductionorder/{id}")]
        public async Task<IActionResult> DeleteProductionOrder (int id)
        {
            var result = await _productionOrderService.DeletePrdouctionOrderAsync(id);
            return Ok(result);
        }
    }
}
