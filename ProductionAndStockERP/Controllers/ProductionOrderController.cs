// Dosya: Controllers/ProductionOrderController.cs
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Dtos.ProductionOrder;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Security.Claims;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,Producer")]
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

        [HttpGet] 
        public async Task<IActionResult> GetAllProductionOrders()
        {
            var result = await _productionOrderService.GetAllPrdouctionOrderAsync();
            return Ok(result);
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> GetProductionOrderById(int id)
        {
            var result = await _productionOrderService.GetPrdouctionOrderByIdAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpPost] 
        public async Task<IActionResult> CreateProductionOrder([FromBody] ProductionOrderCreateDto createDto)
        {
            var userId = User.GetUserId();
            if (userId == null) return BadRequest("Kullanıcı kimliği token'da bulunamadı.");

            var productionOrder = _mapper.Map<ProductionOrder>(createDto);
            productionOrder.Status = Status.Started;
            productionOrder.CreatedAt = DateTime.UtcNow; 
            productionOrder.CreatedBy = userId.Value;

            var result = await _productionOrderService.CreatePrdouctionOrderAsync(productionOrder, userId.Value);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateProductionOrder(int id, [FromBody] ProductionOrderUpdateDto updateDto)
        {
            var userId = User.GetUserId();
            if (userId == null) return BadRequest("Kullanıcı kimliği token'da bulunamadı.");

            var response = await _productionOrderService.GetPrdouctionOrderByIdAsync(id);
            if (!response.Success) return NotFound(response);

            var orderToUpdate = response.Data;
            _mapper.Map(updateDto, orderToUpdate);

            var result = await _productionOrderService.UpdatePrdouctionOrderAsync(orderToUpdate, userId.Value);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeleteProductionOrder(int id)
        {
            var userId = User.GetUserId();
            if (userId == null) return BadRequest("Kullanıcı kimliği token'da bulunamadı.");

            var result = await _productionOrderService.DeletePrdouctionOrderAsync(id, userId.Value);

            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}