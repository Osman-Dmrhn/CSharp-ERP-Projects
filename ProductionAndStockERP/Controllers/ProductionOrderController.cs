using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Dtos.ProductionOrder;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using ProductionAndStockERP.Services;
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
        private readonly IActivityLogsService _activityLogsService;

        public ProductionOrderController(IProductionOrderService productionOrderService, IMapper mapper, IActivityLogsService activityLogsService)
        {
            _productionOrderService = productionOrderService;
            _mapper = mapper;
            _activityLogsService = activityLogsService;
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

            var userId = User.GetUserId();
            if (userId is not null)
            {
                productionOrder.CreatedBy = userId.Value;
                await _activityLogsService.AddLogAsync(userId.Value, $"Kullanıcı Üretim Siparişi Oluşturdu.Üretim Siparişi:{productionOrder.ProductionId}");
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

            var userId = User.GetUserId();
            if (userId is not null)
            {
                await _activityLogsService.AddLogAsync(userId.Value, $"Kullanıcı Üretim Siparişini Güncelledi.Üretim Siparişi:{productionorder.Data.ProductionId}");
            }
            else
            {
                return BadRequest("Kullanıcı kimliği token'da bulunamadı.");
            }

            productionorder.Data=_mapper.Map(newdata, productionorder.Data);
            var result = await _productionOrderService.UpdatePrdouctionOrderAsync(productionorder.Data);
            return Ok(result);
        }

        [HttpPost("deleteproductionorder/{id}")]
        public async Task<IActionResult> DeleteProductionOrder (int id)
        {
            var userId = User.GetUserId();
            if (userId is not null)
            {
                await _activityLogsService.AddLogAsync(userId.Value, $"Kullanıcı Üretim Siparişini Sildi.Üretim Siparişi:{_productionOrderService.GetPrdouctionOrderByIdAsync(id)}");
            }
            else
            {
                return BadRequest("Kullanıcı kimliği token'da bulunamadı.");
            }


            var result = await _productionOrderService.DeletePrdouctionOrderAsync(id);
            return Ok(result);
        }
    }
}
