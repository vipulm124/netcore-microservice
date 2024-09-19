using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wpm.Management.Api.DataAccess;

namespace Wpm.Management.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PetsController : ControllerBase
{
    ManagementDbContext _dbContext;
    public PetsController(ManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var all = await _dbContext.Pets.Include(p => p.Breed).ToListAsync();
        return Ok(all);
    }

    [HttpGet("{Id}", Name = nameof(GetPetById))]
    public async Task<IActionResult> GetPetById(int Id)
    {
        var pet = _dbContext.Pets.Include(p => p.Breed).Where(p => p.Id == Id).FirstOrDefault();
        return Ok(pet);
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewPet newPet)
    {
        var pet = newPet.ToPet();
        await _dbContext.Pets.AddAsync(pet);
        await _dbContext.SaveChangesAsync();
        return CreatedAtRoute(nameof(GetPetById), new { id = pet.Id }, newPet);
    }
}

public record NewPet(string Name, int Age, int BreedId)
{
    public Pet ToPet()
    {
        return new Pet() { Name = Name, Age = Age, BreedId = BreedId };
    }
}
