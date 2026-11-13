using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmApp.Data;
using FarmApp.Models;
using FarmApp.DTOs;

namespace FarmApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnimalsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/animals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAnimals()
        {
            return await _context.Animals
                .Select(a => new AnimalDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Type = a.Type,
                    Breed = a.Breed,
                    BirthDate = a.BirthDate,
                    Gender = a.Gender,
                    HealthStatus = a.HealthStatus,
                    LastVaccinationDate = a.LastVaccinationDate
                })
                .ToListAsync();
        }

        // GET: api/animals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnimalDto>> GetAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            var animalDto = new AnimalDto
            {
                Id = animal.Id,
                Name = animal.Name,
                Type = animal.Type,
                Breed = animal.Breed,
                BirthDate = animal.BirthDate,
                Gender = animal.Gender,
                HealthStatus = animal.HealthStatus,
                LastVaccinationDate = animal.LastVaccinationDate
            };

            return animalDto;
        }

        // POST: api/animals
        [HttpPost]
        public async Task<ActionResult<AnimalDto>> CreateAnimal(CreateAnimalDto dto)
        {
            var animal = new Animal
            {
                Name = dto.Name,
                Type = dto.Type,
                Breed = dto.Breed,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                HealthStatus = dto.HealthStatus,
                LastVaccinationDate = dto.LastVaccinationDate
            };

            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            var animalDto = new AnimalDto
            {
                Id = animal.Id,
                Name = animal.Name,
                Type = animal.Type,
                Breed = animal.Breed,
                BirthDate = animal.BirthDate,
                Gender = animal.Gender,
                HealthStatus = animal.HealthStatus,
                LastVaccinationDate = animal.LastVaccinationDate
            };

            return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animalDto);
        }

        // PUT: api/animals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnimal(int id, CreateAnimalDto dto)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            animal.Name = dto.Name;
            animal.Type = dto.Type;
            animal.Breed = dto.Breed;
            animal.BirthDate = dto.BirthDate;
            animal.Gender = dto.Gender;
            animal.HealthStatus = dto.HealthStatus;
            animal.LastVaccinationDate = dto.LastVaccinationDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/animals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}