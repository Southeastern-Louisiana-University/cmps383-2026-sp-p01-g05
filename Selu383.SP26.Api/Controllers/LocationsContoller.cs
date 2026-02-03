using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Selu383.SP26.Api.Data;
using Selu383.SP26.Api.Dtos;
using Selu383.SP26.Api.Models;



namespace Selu383.SP26.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly DataContext _context;
        public LocationsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/locations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations()
        {
            var locations = await _context.Locations
                .Select(x => new LocationDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    TableCount = x.TableCount
                })
                .ToListAsync();

            return Ok(locations);
        }

        [HttpGet("{id}")]
        public ActionResult<LocationDto> GetLocationById(int id)
        {
            var location = _context.Locations.FirstOrDefault(l => l.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            var locationDto = new LocationDto
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
                TableCount = location.TableCount
            };

            return Ok(locationDto);

        }
        [HttpPost]
        public ActionResult<LocationDto> CreateLocation([FromBody] CreateLocationDto dto)
        {
            var location = new Location
            {
                Name = dto.Name,
                Address = dto.Address,
                TableCount = dto.TableCount
            };
            _context.Locations.Add(location);
            _context.SaveChanges();
            var locationDto = new LocationDto
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
                TableCount = location.TableCount

            };
            return CreatedAtAction(nameof(GetLocationById), new { id = locationDto.Id }, locationDto);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateLocation(int id, [FromBody] UpdateLocationDto dto)
        {
            var location = _context.Locations.FirstOrDefault(l => l.Id == id);
            if (location == null)
            {
                return NotFound();
            }
            // Model validation happens automatically because of [ApiController]

            location.Name = dto.Name;
            location.Address = dto.Address;
            location.TableCount = dto.TableCount;

            _context.SaveChanges();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteLocation(int id)
        {
            var location = _context.Locations.FirstOrDefault(l => l.Id == id);

            if (location == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(location);
            _context.SaveChanges();

            return NoContent();
        }


    }

}
    


