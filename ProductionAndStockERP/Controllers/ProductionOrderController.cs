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
        public async Task<IActionResult> GetAllProductionOrders([FromQuery] ProductionOrderFilterParameters filters)
        {
            var result = await _productionOrderService.GetAllPrdouctionOrderAsync(filters);
            if (result.Success)
            {
                Response.AddPaginationHeader(result.Data.CurrentPage, result.Data.PageSize, result.Data.TotalCount, result.Data.TotalPages);
                return Ok(result.Data.Items);
            }
            return BadRequest(result);
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

            var productionOrderToUpdate = _mapper.Map<ProductionOrder>(updateDto);
            productionOrderToUpdate.ProductionId = id;

            var result = await _productionOrderService.UpdatePrdouctionOrderAsync(productionOrderToUpdate, userId.Value);

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