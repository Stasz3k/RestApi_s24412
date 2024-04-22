using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi_s24412.DTOs;
using RestApi_s24412.Models;
using RestApi_s24412;

[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly YourDbContext _context;

    public AnimalsController(YourDbContext context)
    {
        _context = context;
    }

    // GET: api/animals?orderBy=name
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAnimals(string orderBy = "name")
    {
        var animals = _context.Animals.AsQueryable();

        switch (orderBy.ToLower())
        {
            case "name":
                animals = animals.OrderBy(a => a.Name);
                break;
            case "category":
                animals = animals.OrderBy(a => a.Category);
                break;
            case "area":
                animals = animals.OrderBy(a => a.Area);
                break;
        }

        return await animals.Select(a => new AnimalDto
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            Category = a.Category,
            Area = a.Area
        }).ToListAsync();
    }

    // POST: api/animals
    [HttpPost]
    public async Task<ActionResult<Animal>> PostAnimal(AnimalDto animalDto)
    {
        var animal = new Animal
        {
            Name = animalDto.Name,
            Description = animalDto.Description,
            Category = animalDto.Category,
            Area = animalDto.Area
        };
        _context.Animals.Add(animal);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
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

        return new AnimalDto
        {
            Id = animal.Id,
            Name = animal.Name,
            Description = animal.Description,
            Category = animal.Category,
            Area = animal.Area
        };
    }

    // PUT: api/animals/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAnimal(int id, AnimalDto animalDto)
    {
        if (id != animalDto.Id)
        {
            return BadRequest();
        }

        var animal = await _context.Animals.FindAsync(id);
        if (animal == null)
        {
            return NotFound();
        }

        animal.Name = animalDto.Name;
        animal.Description = animalDto.Description;
        animal.Category = animalDto.Category;
        animal.Area = animalDto.Area;

        _context.Entry(animal).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Animals.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

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
