using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionAndStockERP.Dtos.ProductDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Security.Claims;

namespace ProductionAndStockERP.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductFilterParameters filters)
        {
            var result = await _productService.GetAllProductsAsync(filters);
            if (result.Success)
            {
                Response.AddPaginationHeader(result.Data.CurrentPage, result.Data.PageSize, result.Data.TotalCount, result.Data.TotalPages);
                return Ok(result.Data.Items);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto createDto)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            var product = _mapper.Map<Product>(createDto);
            var result = await _productService.CreateProductAsync(product, userId.Value);

            if (!result.Success) return BadRequest(result);
            return CreatedAtAction(nameof(GetProductById), new { id = result.Data.ProductId }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto updateDto)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            var response = await _productService.GetProductByIdAsync(id);
            if (!response.Success) return NotFound(response);

            var productToUpdate = _mapper.Map<Product>(response.Data);
            _mapper.Map(updateDto, productToUpdate);
            productToUpdate.ProductId = id;

            var result = await _productService.UpdateProductAsync(productToUpdate, userId.Value);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _productService.DeleteProductAsync(id, userId.Value);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}