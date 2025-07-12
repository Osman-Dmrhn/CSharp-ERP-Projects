using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Dtos.OrderDtos;
using ProductionAndStockERP.Dtos.UserDtos;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Security.Claims;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IOrderService orderService,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet("getallorders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return Ok(result);
        }

        [HttpGet("getorderbyid")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("createorder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateUserDto newdata)
        {
            var order= _mapper.Map<Order>(newdata);
            order.CreatedAt= DateTime.Now;
            order.Status = OrderStatus.Pending;

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(int.TryParse(userIdClaim, out int userId))
            {
                order.UserId = userId;
            }
            else
            {
                return BadRequest("Kullanıcı kimliği token'da bulunamadı.");
            }

            var result = await _orderService.CreateOrderAsync(order);
            return Ok(result);
        }

        [HttpPost("updateorder/{id}")]
        public async Task<IActionResult> UpdateOrder(int id,[FromBody] OrderUpdateDto newdata)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if(order.Data ==null)
            {
                return Ok(order);
            }
            _mapper.Map(newdata, order.Data);
            var result = await _orderService.UpdateOrderAsync(order.Data);
            return Ok(result);
        }

        [HttpPost("deleteorder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result =await _orderService.DeleteOrderAsync(id);
            return Ok(result);
        }
    }
}
