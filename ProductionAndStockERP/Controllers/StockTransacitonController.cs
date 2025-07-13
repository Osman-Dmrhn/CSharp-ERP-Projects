using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Dtos.StockTransactionDtos;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Route("api/stocktransaction")]
    public class StockTransacitonController : ControllerBase
    {
        private  readonly IStockTransactionService _stockTransactionService;
        private readonly IMapper _mapper;

        public StockTransacitonController(IStockTransactionService stockTransactionService, IMapper mapper)
        {
            _stockTransactionService = stockTransactionService;
            _mapper = mapper;
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
            var result = await _stockTransactionService.CreateStockTransactionAsync(stockTransaction);
            return Ok(result);
        }

        [HttpPost("updatestocktransaction/{id}")]
        public async Task<IActionResult> UpdateStockTransaction(int id,[FromBody] StockTransactionCreate newdata)
        {
            var stockTransaction = await _stockTransactionService.GetStockTransactionByIdAsync(id);
            if(stockTransaction.Data == null) 
                return Ok(stockTransaction);

            stockTransaction.Data =_mapper.Map(newdata, stockTransaction.Data);

            var result = await _stockTransactionService.UpdateStockTransactionAsync(stockTransaction.Data);

            return Ok(result);

        }

        [HttpPost("deletestocktransaction/{id}")]
        public async Task<IActionResult> DeleteStockTransaction(int id)
        {
            var result=await _stockTransactionService.DeleteStockTransactionAsync(id);
            return Ok(result);
        }
    }
}
