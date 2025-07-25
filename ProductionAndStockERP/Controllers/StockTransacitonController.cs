// Dosya: Controllers/StockTransactionController.cs
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Dtos.StockTransactionDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Security.Claims;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Authorize(Roles ="Admin,Producer")]
    [Route("api/stocktransactions")]
    public class StockTransactionController : ControllerBase
    {
        private readonly IStockTransactionService _stockTransactionService;
        private readonly IMapper _mapper;

        public StockTransactionController(IStockTransactionService stockTransactionService, IMapper mapper)
        {
            _stockTransactionService = stockTransactionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStockTransactions([FromQuery] StockTransactionFilterParameters filters)
        {
            var result = await _stockTransactionService.GetAllStockTransactionAsync(filters);
            if (result.Success)
            {
                Response.AddPaginationHeader(result.Data.CurrentPage, result.Data.PageSize, result.Data.TotalCount, result.Data.TotalPages);
                return Ok(result.Data.Items);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockTransactionById(int id)
        {
            var result = await _stockTransactionService.GetStockTransactionByIdAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStockTransaction([FromBody] StockTransactionCreate createDto)
        {
            var userId = User.GetUserId();
            if (userId == null) return BadRequest("Kullanıcı kimliği token'da bulunamadı.");

            var stockTransaction = _mapper.Map<StockTransaction>(createDto);
            stockTransaction.CreatedAt = DateTime.UtcNow;

            var result = await _stockTransactionService.CreateStockTransactionAsync(stockTransaction, userId.Value);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStockTransaction(int id, [FromBody] StockTransactionUpdateDto updateDto)
        {
            var userId = User.GetUserId();
            if (userId == null) return BadRequest("Kullanıcı kimliği token'da bulunamadı.");

            var transactionToUpdate = _mapper.Map<StockTransaction>(updateDto);
            transactionToUpdate.StockTxnId = id;

            var result = await _stockTransactionService.UpdateStockTransactionAsync(transactionToUpdate, userId.Value);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockTransaction(int id)
        {
            var userId = User.GetUserId();
            if (userId == null) return BadRequest("Kullanıcı kimliği token'da bulunamadı.");

            var result = await _stockTransactionService.DeleteStockTransactionAsync(id, userId.Value);

            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}