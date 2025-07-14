using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Dtos.StockTransactionDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using ProductionAndStockERP.Services;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,Producer")]
    [Route("api/stocktransaction")]
    public class StockTransacitonController : ControllerBase
    {
        private  readonly IStockTransactionService _stockTransactionService;
        private readonly IMapper _mapper;
        private readonly IActivityLogsService _activityLogsService;

        public StockTransacitonController(IStockTransactionService stockTransactionService, IMapper mapper, IActivityLogsService activityLogsService)
        {
            _stockTransactionService = stockTransactionService;
            _mapper = mapper;
            _activityLogsService = activityLogsService;
        }

        [HttpGet("getallstocktransaction")]
        public async Task<IActionResult> GetAllStockTransactions()
        {
            var result = await _stockTransactionService.GetAllStockTransactionAsync();
            return Ok(result);
        }

        [HttpGet("getstocktransaction/{id}")]
        public async Task<IActionResult> GetStockTransactionById(int id)
        {
            var result = await _stockTransactionService.GetStockTransactionByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("createstocktransaction")]
        public async Task<IActionResult> CreateStockTransaction([FromBody] StockTransactionCreate newdata)
        {
            var stockTransaction =_mapper.Map<StockTransaction>(newdata);
            stockTransaction.CreatedAt= DateTime.Now;

            var userId = User.GetUserId();
            if (userId is not null)
            {
                await _activityLogsService.AddLogAsync(userId.Value, $"Kullanıcı Stok Hareketi Ekledi.Stok Hareketi:{stockTransaction.StockTxnId}");
            }
            else
            {
                return BadRequest("Kullanıcı kimliği token'da bulunamadı.");
            }


            var result = await _stockTransactionService.CreateStockTransactionAsync(stockTransaction);
            return Ok(result);
        }

        [HttpPost("updatestocktransaction/{id}")]
        public async Task<IActionResult> UpdateStockTransaction(int id,[FromBody] StockTransactionCreate newdata)
        {
            var stockTransaction = await _stockTransactionService.GetStockTransactionByIdAsync(id);
            if(stockTransaction.Data == null) 
                return Ok(stockTransaction);

            var userId = User.GetUserId();
            if (userId is not null)
            {
                await _activityLogsService.AddLogAsync(userId.Value, $"Kullanıcı Stok Hareketini Güncelledi.Stok Hareketi:{stockTransaction.Data.StockTxnId}");
            }
            else
            {
                return BadRequest("Kullanıcı kimliği token'da bulunamadı.");
            }

            stockTransaction.Data =_mapper.Map(newdata, stockTransaction.Data);

            var result = await _stockTransactionService.UpdateStockTransactionAsync(stockTransaction.Data);

            return Ok(result);

        }

        [HttpPost("deletestocktransaction/{id}")]
        public async Task<IActionResult> DeleteStockTransaction(int id)
        {
            var userId = User.GetUserId();
            if (userId is not null)
            {
                await _activityLogsService.AddLogAsync(userId.Value, $"Kullanıcı Stok Hareketni Sildi.Stok Hareketi:{_stockTransactionService.GetStockTransactionByIdAsync(id)}");
            }
            else
            {
                return BadRequest("Kullanıcı kimliği token'da bulunamadı.");
            }

            var result=await _stockTransactionService.DeleteStockTransactionAsync(id);
            return Ok(result);
        }
    }
}
