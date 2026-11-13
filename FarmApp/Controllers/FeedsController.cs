using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmApp.Data;
using FarmApp.Models;
using FarmApp.DTOs;

namespace FarmApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/feeds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedDto>>> GetFeeds()
        {
            return await _context.Feeds
                .Select(f => new FeedDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Type = f.Type,
                    Quantity = f.Quantity,
                    Unit = f.Unit,
                    ExpiryDate = f.ExpiryDate
                })
                .ToListAsync();
        }

        // GET: api/feeds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedDto>> GetFeed(int id)
        {
            var feed = await _context.Feeds.FindAsync(id);
            if (feed == null)
            {
                return NotFound();
            }

            var feedDto = new FeedDto
            {
                Id = feed.Id,
                Name = feed.Name,
                Type = feed.Type,
                Quantity = feed.Quantity,
                Unit = feed.Unit,
                ExpiryDate = feed.ExpiryDate
            };

            return feedDto;
        }

        // POST: api/feeds
        [HttpPost]
        public async Task<ActionResult<FeedDto>> CreateFeed(CreateFeedDto dto)
        {
            var feed = new Feed
            {
                Name = dto.Name,
                Type = dto.Type,
                Quantity = dto.Quantity,
                Unit = dto.Unit,
                ExpiryDate = dto.ExpiryDate
            };

            _context.Feeds.Add(feed);
            await _context.SaveChangesAsync();

            var feedDto = new FeedDto
            {
                Id = feed.Id,
                Name = feed.Name,
                Type = feed.Type,
                Quantity = feed.Quantity,
                Unit = feed.Unit,
                ExpiryDate = feed.ExpiryDate
            };

            return CreatedAtAction(nameof(GetFeed), new { id = feed.Id }, feedDto);
        }

        // PUT: api/feeds/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeed(int id, CreateFeedDto dto)
        {
            var feed = await _context.Feeds.FindAsync(id);
            if (feed == null)
            {
                return NotFound();
            }

            feed.Name = dto.Name;
            feed.Type = dto.Type;
            feed.Quantity = dto.Quantity;
            feed.Unit = dto.Unit;
            feed.ExpiryDate = dto.ExpiryDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/feeds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeed(int id)
        {
            var feed = await _context.Feeds.FindAsync(id);
            if (feed == null)
            {
                return NotFound();
            }

            bool hasConsumptions = await _context.FeedConsumptions.AnyAsync(fc => fc.FeedId == id);
            if (hasConsumptions)
            {
                return BadRequest("Нельзя удалить корм, так как есть записи о его расходе.");
            }

            _context.Feeds.Remove(feed);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/feeds/5/consume
        [HttpPost("{id}/consume")]
        public async Task<IActionResult> ConsumeFeed(int id, FeedConsumptionDto dto)
        {
            var feed = await _context.Feeds.FindAsync(id);
            if (feed == null)
            {
                return NotFound();
            }

            if (feed.Quantity < dto.Quantity)
            {
                return BadRequest($"Недостаточно корма. Доступно: {feed.Quantity} {feed.Unit}");
            }

            feed.Quantity -= dto.Quantity;

            var consumption = new FeedConsumption
            {
                FeedId = id,
                Date = dto.Date,
                Quantity = dto.Quantity,
                Notes = dto.Notes
            };
            _context.FeedConsumptions.Add(consumption);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}