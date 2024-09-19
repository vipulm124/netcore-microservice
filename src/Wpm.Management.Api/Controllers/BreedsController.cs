using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wpm.Management.Api.DataAccess;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Wpm.Management.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class BreedsController : ControllerBase
{
    ManagementDbContext _dbContext;
    ILogger<BreedsController> _logger;

    public BreedsController(ManagementDbContext dbContext, ILogger<BreedsController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var all = await _dbContext.Breeds.ToListAsync();
        return all != null ? Ok(all) : NotFound(); ;
    }

    [HttpGet("{id}", Name = nameof(GetBreedById))]
    public async Task<IActionResult> GetBreedById(int id)
    {
        var breed = _dbContext.Breeds.Where(x => x.Id == id).FirstOrDefault();
        return breed != null ?  Ok(breed) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewBreed newBreed)
    {
        try
        {

        
        var breed = newBreed.ToBreed();
        await _dbContext.Breeds.AddAsync(breed);
        await _dbContext.SaveChangesAsync();

        return CreatedAtRoute(nameof(GetBreedById), new { id = breed.Id }, newBreed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

    }


}


public record NewBreed(string Name)
{
    public Breed ToBreed()
    {
        return new Breed(0,Name);
    }
}

