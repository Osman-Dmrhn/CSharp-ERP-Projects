using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Interfaces;

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
    }
}
