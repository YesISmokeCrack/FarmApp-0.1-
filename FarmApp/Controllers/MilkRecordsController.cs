using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmApp.Data;
using FarmApp.Models;
using FarmApp.DTOs;

namespace FarmApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MilkRecordsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MilkRecordsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/milkrecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MilkRecordDto>>> GetMilkRecords()
        {
            return await _context.MilkRecords
                .Include(m => m.Animal)
                .Select(m => new MilkRecordDto
                {
                    Id = m.Id,
                    AnimalId = m.AnimalId,
                    AnimalName = m.Animal.Name,
                    Date = m.Date,
                    Quantity = m.Quantity,
                    FatPercentage = m.FatPercentage,
                    ProteinPercentage = m.ProteinPercentage
                })
                .ToListAsync();
        }

        // GET: api/milkrecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MilkRecordDto>> GetMilkRecord(int id)
        {
            var milkRecord = await _context.MilkRecords
                .Include(m => m.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (milkRecord == null)
            {
                return NotFound();
            }

            var dto = new MilkRecordDto
            {
                Id = milkRecord.Id,
                AnimalId = milkRecord.AnimalId,
                AnimalName = milkRecord.Animal.Name,
                Date = milkRecord.Date,
                Quantity = milkRecord.Quantity,
                FatPercentage = milkRecord.FatPercentage,
                ProteinPercentage = milkRecord.ProteinPercentage
            };

            return dto;
        }

        // GET: api/milkrecords/animal/5
        [HttpGet("animal/{animalId}")]
        public async Task<ActionResult<IEnumerable<MilkRecordDto>>> GetMilkRecordsByAnimal(int animalId)
        {
            return await _context.MilkRecords
                .Where(m => m.AnimalId == animalId)
                .Include(m => m.Animal)
                .Select(m => new MilkRecordDto
                {
                    Id = m.Id,
                    AnimalId = m.AnimalId,
                    AnimalName = m.Animal.Name,
                    Date = m.Date,
                    Quantity = m.Quantity,
                    FatPercentage = m.FatPercentage,
                    ProteinPercentage = m.ProteinPercentage
                })
                .ToListAsync();
        }

        // POST: api/milkrecords
        [HttpPost]
        public async Task<ActionResult<MilkRecordDto>> CreateMilkRecord(CreateMilkRecordDto dto)
        {
            var animal = await _context.Animals.FindAsync(dto.AnimalId);
            if (animal == null)
            {
                return BadRequest($"Животное с ID {dto.AnimalId} не найдено.");
            }

            var milkRecord = new MilkRecord
            {
                AnimalId = dto.AnimalId,
                Date = dto.Date,
                Quantity = dto.Quantity,
                FatPercentage = dto.FatPercentage,
                ProteinPercentage = dto.ProteinPercentage
            };

            _context.MilkRecords.Add(milkRecord);
            await _context.SaveChangesAsync();

            // Загружаем связанное животное
            await _context.Entry(milkRecord).Reference(m => m.Animal).LoadAsync();

            var dtoResult = new MilkRecordDto
            {
                Id = milkRecord.Id,
                AnimalId = milkRecord.AnimalId,
                AnimalName = milkRecord.Animal.Name,
                Date = milkRecord.Date,
                Quantity = milkRecord.Quantity,
                FatPercentage = milkRecord.FatPercentage,
                ProteinPercentage = milkRecord.ProteinPercentage
            };

            return CreatedAtAction(nameof(GetMilkRecord), new { id = milkRecord.Id }, dtoResult);
        }

        // PUT: api/milkrecords/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMilkRecord(int id, CreateMilkRecordDto dto)
        {
            var milkRecord = await _context.MilkRecords.FindAsync(id);
            if (milkRecord == null)
            {
                return NotFound();
            }

            // Если меняется животное, проверим его существование
            if (milkRecord.AnimalId != dto.AnimalId)
            {
                var animal = await _context.Animals.FindAsync(dto.AnimalId);
                if (animal == null)
                {
                    return BadRequest("Животное не найдено.");
                }
            }

            milkRecord.AnimalId = dto.AnimalId;
            milkRecord.Date = dto.Date;
            milkRecord.Quantity = dto.Quantity;
            milkRecord.FatPercentage = dto.FatPercentage;
            milkRecord.ProteinPercentage = dto.ProteinPercentage;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/milkrecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMilkRecord(int id)
        {
            var milkRecord = await _context.MilkRecords.FindAsync(id);
            if (milkRecord == null)
            {
                return NotFound();
            }

            _context.MilkRecords.Remove(milkRecord);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}