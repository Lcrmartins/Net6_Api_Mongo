using Net6.Api.Mongo.Collections;
using Net6.Api.Mongo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Net6.Api.Mongo.Controllers;


[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    private readonly IPeopleRepository _ipeopleRepository;

    public PeopleController(IPeopleRepository ipeopleRepository)
    {
        _ipeopleRepository = ipeopleRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var people = await _ipeopleRepository.GetAllAsync();
        return Ok(people);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var people = await _ipeopleRepository.GetByIdAsync(id);
        if (people == null)
        {
            return NotFound();
        } 
        return Ok(people);
    }

    [HttpPost]
    public async Task<IActionResult> Post(People newPeople)
    {
        await _ipeopleRepository.CreateNewPeopleAsync(newPeople);
        return CreatedAtAction(nameof(Get), new { id = newPeople.Id }, newPeople);
    }

    [HttpPut]
    public async Task<IActionResult> Put(People updatePeople)
    {
        var people = await _ipeopleRepository.GetByIdAsync(updatePeople.Id);
        if (people == null)
        {
            return NotFound();
        }
        await _ipeopleRepository.UpdatePeopleAsync(updatePeople);
        return Ok("Object updated.");
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var people = await _ipeopleRepository.GetByIdAsync(id);
        if (people == null)
        {
            return NotFound();
        }
        await _ipeopleRepository.DeletePeopleAsync(id);
        return Ok("Object deleted.");
    }
}