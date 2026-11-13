using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmApp.Data;
using FarmApp.DTOs;
using FarmApp.Models;

namespace FarmApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetSales()
        {
            return await _context.Sales
                .Include(s => s.Product)
                .Select(s => new SaleDto
                {
                    Id = s.Id,
                    ProductId = s.ProductId,
                    ProductName = s.Product.Name,
                    Customer = s.Customer,
                    Quantity = s.Quantity,
                    SaleDate = s.SaleDate,
                    PricePerUnit = s.PricePerUnit,
                    TotalAmount = s.TotalAmount
                })
                .ToListAsync();
        }

        // GET: api/sales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleDto>> GetSale(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.Product)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
            {
                return NotFound();
            }

            var saleDto = new SaleDto
            {
                Id = sale.Id,
                ProductId = sale.ProductId,
                ProductName = sale.Product.Name,
                Customer = sale.Customer,
                Quantity = sale.Quantity,
                SaleDate = sale.SaleDate,
                PricePerUnit = sale.PricePerUnit,
                TotalAmount = sale.TotalAmount
            };

            return saleDto;
        }

        // POST: api/sales
        [HttpPost]
        public async Task<ActionResult<SaleDto>> CreateSale(CreateSaleDto dto)
        {
            // Проверяем продукт
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
            {
                return BadRequest($"Продукт с ID {dto.ProductId} не найден.");
            }

            // Ищем складской остаток
            var warehouseItem = await _context.WarehouseItems
                .FirstOrDefaultAsync(w => w.ProductId == dto.ProductId);
            if (warehouseItem == null)
            {
                return BadRequest("Данный продукт отсутствует на складе.");
            }

            // Проверяем количество
            if (warehouseItem.Quantity < dto.Quantity)
            {
                return BadRequest($"Недостаточно товара на складе. Доступно: {warehouseItem.Quantity} {product.Unit}");
            }

            // Списание
            warehouseItem.Quantity -= dto.Quantity;

            // Создаём продажу
            var sale = new Sale
            {
                ProductId = dto.ProductId,
                Customer = dto.Customer,
                Quantity = dto.Quantity,
                SaleDate = dto.SaleDate,
                PricePerUnit = dto.PricePerUnit
            };
            _context.Sales.Add(sale);

            // Сохраняем
            await _context.SaveChangesAsync();

            // Загружаем продукт для ответа
            await _context.Entry(sale).Reference(s => s.Product).LoadAsync();

            var saleDto = new SaleDto
            {
                Id = sale.Id,
                ProductId = sale.ProductId,
                ProductName = sale.Product.Name,
                Customer = sale.Customer,
                Quantity = sale.Quantity,
                SaleDate = sale.SaleDate,
                PricePerUnit = sale.PricePerUnit,
                TotalAmount = sale.TotalAmount
            };

            return CreatedAtAction(nameof(GetSale), new { id = sale.Id }, saleDto);
        }
    }
}