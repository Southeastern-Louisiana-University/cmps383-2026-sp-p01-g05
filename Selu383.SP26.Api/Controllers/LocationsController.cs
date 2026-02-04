using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Selu383.SP26.Api.Data;
using Selu383.SP26.Api.Dtos;
using Selu383.SP26.Api.Entities;

namespace Selu383.SP26.Api.Controllers;

[Route("api/locations")]
[ApiController]
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

    // GET: api/locations/5
    [HttpGet("{id}")]
    public async Task<ActionResult<LocationDto>> GetLocation(int id)
    {
        var location = await _context.Locations.FindAsync(id);

        if (location == null)
        {
            return NotFound();
        }

        var dto = new LocationDto
        {
            Id = location.Id,
            Name = location.Name,
            Address = location.Address,
            TableCount = location.TableCount
        };

        return Ok(dto);
    }

    // POST: api/locations
    [HttpPost]
    public async Task<ActionResult<LocationDto>> PostLocation(LocationDto dto)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Name is required");
        }

        if (dto.Name.Length > 120)
        {
            return BadRequest("Name cannot exceed 120 characters");
        }

        if (string.IsNullOrWhiteSpace(dto.Address))
        {
            return BadRequest("Address is required");
        }

        if (dto.TableCount < 1)
        {
            return BadRequest("Table count must be at least 1");
        }

        var location = new Location
        {
            Name = dto.Name,
            Address = dto.Address,
            TableCount = dto.TableCount
        };

        _context.Locations.Add(location);
        await _context.SaveChangesAsync();

        dto.Id = location.Id;

        return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, dto);
    }

    // PUT: api/locations/5
    [HttpPut("{id}")]
    public async Task<ActionResult<LocationDto>> PutLocation(int id, LocationDto dto)
    {
        var location = await _context.Locations.FindAsync(id);

        if (location == null)
        {
            return NotFound();
        }

        // Validation
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Name is required");
        }

        if (dto.Name.Length > 120)
        {
            return BadRequest("Name cannot exceed 120 characters");
        }

        if (string.IsNullOrWhiteSpace(dto.Address))
        {
            return BadRequest("Address is required");
        }

        // Update properties
        location.Name = dto.Name;
        location.Address = dto.Address;
        location.TableCount = dto.TableCount;

        await _context.SaveChangesAsync();

        dto.Id = location.Id;

        return Ok(dto);
    }

    // DELETE: api/locations/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        var location = await _context.Locations.FindAsync(id);

        if (location == null)
        {
            return NotFound();
        }

        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();

        return Ok();
    }
}