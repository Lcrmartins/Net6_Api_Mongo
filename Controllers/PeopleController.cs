using Net6.Api.Mongo.Collections;
using Net6.Api.Mongo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Net6.Api.Mongo.Controllers;


[ApiController]
[Route("[controller]")]
public class PeopleController:ControllerBase
{
    private readonly IPeopleRepository _ipeopleRepository;

    public PeopleController(IPeopleRepository ipeopleRepository)
    {
        _ipeopleRepository = ipeopleRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetPeopleAsync()
    {
        var people = await _ipeopleRepository.GetAllAsync();
        return Ok(people);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetPeopleByIdAsync(string id)
    {
        var people = await _ipeopleRepository.GetByIdAsync(id);
        return (people==null)? NotFound() : Ok(people);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(People newPeople)
    {
        await _ipeopleRepository.CreateNewPeopleAsync(newPeople);
        return CreatedAtAction(nameof(GetPeopleByIdAsync), new { id = newPeople.Id }, newPeople);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync(People updatePeople)
    {
        var people = await _ipeopleRepository.GetByIdAsync(updatePeople.Id);
        if (people == null)
        {
            return NotFound();
        }
        await _ipeopleRepository.UpdatePeopleAsync(updatePeople);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var people = await _ipeopleRepository.GetByIdAsync(id);
        if (people == null)
        {
            return NotFound();
        }
        await _ipeopleRepository.DeletePeopleAsync(id);
        return NoContent();
    }
}