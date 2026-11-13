using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmApp.Data;
using FarmApp.Models;
using FarmApp.DTOs;

namespace FarmApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            return await _context.Products
                .Include(p => p.Animal)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type,
                    Quantity = p.Quantity,
                    Unit = p.Unit,
                    ProductionDate = p.ProductionDate,
                    ExpiryDate = p.ExpiryDate,
                    AnimalId = p.AnimalId,
                    AnimalName = p.Animal != null ? p.Animal.Name : null
                })
                .ToListAsync();
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Animal)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Type = product.Type,
                Quantity = product.Quantity,
                Unit = product.Unit,
                ProductionDate = product.ProductionDate,
                ExpiryDate = product.ExpiryDate,
                AnimalId = product.AnimalId,
                AnimalName = product.Animal?.Name
            };

            return productDto;
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto dto)
        {
            if (dto.AnimalId.HasValue)
            {
                var animal = await _context.Animals.FindAsync(dto.AnimalId.Value);
                if (animal == null)
                {
                    return BadRequest($"Животное с ID {dto.AnimalId} не найдено.");
                }
            }

            var product = new Product
            {
                Name = dto.Name,
                Type = dto.Type,
                Quantity = dto.Quantity,
                Unit = dto.Unit,
                ProductionDate = dto.ProductionDate,
                ExpiryDate = dto.ExpiryDate,
                AnimalId = dto.AnimalId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var warehouseItem = new WarehouseItem
            {
                ProductId = product.Id,
                Quantity = product.Quantity,
                Location = "Основной склад"
            };
            _context.WarehouseItems.Add(warehouseItem);
            await _context.SaveChangesAsync();

            if (product.AnimalId.HasValue)
            {
                await _context.Entry(product).Reference(p => p.Animal).LoadAsync();
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Type = product.Type,
                Quantity = product.Quantity,
                Unit = product.Unit,
                ProductionDate = product.ProductionDate,
                ExpiryDate = product.ExpiryDate,
                AnimalId = product.AnimalId,
                AnimalName = product.Animal?.Name
            };

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productDto);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, CreateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            if (dto.AnimalId.HasValue && dto.AnimalId != product.AnimalId)
            {
                var animal = await _context.Animals.FindAsync(dto.AnimalId.Value);
                if (animal == null)
                {
                    return BadRequest("Животное не найдено.");
                }
            }

            product.Name = dto.Name;
            product.Type = dto.Type;
            product.Unit = dto.Unit;
            product.ProductionDate = dto.ProductionDate;
            product.ExpiryDate = dto.ExpiryDate;
            product.AnimalId = dto.AnimalId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var warehouseItem = await _context.WarehouseItems.FirstOrDefaultAsync(w => w.ProductId == id);
            if (warehouseItem != null && warehouseItem.Quantity > 0)
            {
                return BadRequest("Нельзя удалить продукт, так как на складе есть остаток.");
            }

            bool hasSales = await _context.Sales.AnyAsync(s => s.ProductId == id);
            if (hasSales)
            {
                return BadRequest("Нельзя удалить продукт, так как есть продажи.");
            }

            if (warehouseItem != null)
            {
                _context.WarehouseItems.Remove(warehouseItem);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/products/expired
        [HttpGet("expired")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetExpiredProducts()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Products
                .Include(p => p.Animal)
                .Where(p => p.ExpiryDate < today)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type,
                    Quantity = p.Quantity,
                    Unit = p.Unit,
                    ProductionDate = p.ProductionDate,
                    ExpiryDate = p.ExpiryDate,
                    AnimalId = p.AnimalId,
                    AnimalName = p.Animal != null ? p.Animal.Name : null
                })
                .ToListAsync();
        }
    }
}