// Dosya: Controllers/OrderController.cs
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Dtos.OrderDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Security.Claims;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,SalesManager")]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        // Log servisi ve HttpContextAccessor artık burada GEREKLİ DEĞİL.

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto createOrderDto)
        {
            var userId = User.GetUserId();
            if (userId == null) return BadRequest("Kullanıcı kimliği token'da bulunamadı.");

            var order = _mapper.Map<Order>(createOrderDto);
            order.CreatedAt = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;
            order.UserId = userId.Value;

            var result = await _orderService.CreateOrderAsync(order, userId.Value);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderUpdateDto updateDto)
        {
            var userId = User.GetUserId();
            if (userId == null) return BadRequest("Kullanıcı kimliği token'da bulunamadı.");

            var response = await _orderService.GetOrderByIdAsync(id);
            if (!response.Success) return NotFound(response);

            var orderToUpdate = response.Data;
            _mapper.Map(updateDto, orderToUpdate);

            var result = await _orderService.UpdateOrderAsync(orderToUpdate, userId.Value);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var userId = User.GetUserId();
            if (userId == null) return BadRequest("Kullanıcı kimliği token'da bulunamadı.");

            var result = await _orderService.DeleteOrderAsync(id, userId.Value);

            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
