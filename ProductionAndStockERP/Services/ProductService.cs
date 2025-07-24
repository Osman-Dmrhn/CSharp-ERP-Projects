using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductionAndStockERP.Data;
using ProductionAndStockERP.Dtos.ProductDtos;
using ProductionAndStockERP.Helpers;
using ProductionAndStockERP.Interfaces;
using ProductionAndStockERP.Models;
using System.Linq.Dynamic.Core;
using System.Text.Json;

namespace ProductionAndStockERP.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IActivityLogsService _activityLogsService;
        private readonly IMapper _mapper;

        public ProductService(ApplicationDbContext context, IActivityLogsService activityLogsService, IMapper mapper)
        {
            _context = context;
            _activityLogsService = activityLogsService;
            _mapper = mapper;
        }

        public async Task<ResponseHelper<PagedResponse<ProductDto>>> GetAllProductsAsync(ProductFilterParameters filters)
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(filters.Name))
                query = query.Where(p => p.Name.Contains(filters.Name));
            if (!string.IsNullOrEmpty(filters.SortOrder))
                query = query.OrderBy(filters.SortOrder);
            else
                query = query.OrderBy(p => p.Name);

            var dtoQuery = query.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            });

            var totalRecords = await dtoQuery.CountAsync();
            var items = await dtoQuery.Skip((filters.PageNumber - 1) * filters.PageSize).Take(filters.PageSize).ToListAsync();
            var pagedResponse = new PagedResponse<ProductDto>(items, filters.PageNumber, filters.PageSize, totalRecords);
            return ResponseHelper<PagedResponse<ProductDto>>.Ok(pagedResponse);
        }

        public async Task<ResponseHelper<ProductDto>> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return ResponseHelper<ProductDto>.Fail("Ürün bulunamadı.");
            var productDto = _mapper.Map<ProductDto>(product);
            return ResponseHelper<ProductDto>.Ok(productDto);
        }

        public async Task<ResponseHelper<ProductDto>> CreateProductAsync(Product product, int performingUserId)
        {
            if (await _context.Products.AnyAsync(p => p.Name == product.Name))
            {
                return ResponseHelper<ProductDto>.Fail("Bu isimde bir ürün zaten mevcut.");
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            await _activityLogsService.AddLogAsync(performingUserId, "Yeni ürün oluşturuldu.", "Başarılı", "Product", product.ProductId.ToString());
            var productDto = _mapper.Map<ProductDto>(product);
            return ResponseHelper<ProductDto>.Ok(productDto, "Ürün başarıyla oluşturuldu.");
        }

        public async Task<ResponseHelper<ProductDto>> UpdateProductAsync(Product updatedProduct, int performingUserId)
        {
            var existingProduct = await _context.Products.FindAsync(updatedProduct.ProductId);
            if (existingProduct == null) return ResponseHelper<ProductDto>.Fail("Güncellenecek ürün bulunamadı.");

            var changes = new Dictionary<string, object>();
            if (existingProduct.Name != updatedProduct.Name) changes["Name"] = new { Old = existingProduct.Name, New = updatedProduct.Name };
            if (existingProduct.Description != updatedProduct.Description) changes["Description"] = new { Old = existingProduct.Description, New = updatedProduct.Description };

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            await _context.SaveChangesAsync();

            string changesJson = changes.Count > 0 ? JsonSerializer.Serialize(changes) : null;
            await _activityLogsService.AddLogAsync(performingUserId, "Ürün güncellendi.", "Başarılı", "Product", existingProduct.ProductId.ToString(), changesJson);
            var productDto = _mapper.Map<ProductDto>(existingProduct);
            return ResponseHelper<ProductDto>.Ok(productDto, "Ürün başarıyla güncellendi.");
        }

        public async Task<ResponseHelper<bool>> DeleteProductAsync(int id, int performingUserId)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                await _activityLogsService.AddLogAsync(performingUserId, $"ID'si {id} olan ürünü silme denemesi başarısız.", "Başarısız", "Product", id.ToString());
                return ResponseHelper<bool>.Fail("Ürün bulunamadı.");
            }
            var isProductInUse = await _context.Orders.AnyAsync(o => o.ProductId == id);
            if (isProductInUse)
            {
                await _activityLogsService.AddLogAsync(performingUserId, $"'{product.Name}' ürününü silme denemesi engellendi (ilişkili sipariş var).", "Başarısız", "Product", id.ToString());
                return ResponseHelper<bool>.Fail("Bu ürün siparişlere bağlı olduğu için silinemez.");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            await _activityLogsService.AddLogAsync(performingUserId, $"'{product.Name}' adlı ürün silindi.", "Başarılı", "Product", id.ToString());
            return ResponseHelper<bool>.Ok(true, "Ürün başarıyla silindi.");
        }
    }
}