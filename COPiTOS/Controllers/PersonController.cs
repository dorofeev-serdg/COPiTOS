using AutoMapper;
using COPiTOS.DTOs;
using COPiTOS.Models;
using COPiTOS.Services;
using Microsoft.AspNetCore.Mvc;

namespace COPiTOS.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PersonController(IPersonService personService, IMapper mapper, ILogger<PersonController> logger)
    : ControllerBase
{
    /// <summary>
    /// Gets all persons
    /// </summary>
    /// <response code="200">All found persons</response>
    /// <response code="500">Server error occurres</response>
    [HttpGet("all")]
    [ProducesResponseType(typeof(PersonsDto), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<PersonsDto>> GetAll()
    {
        try
        {
            var result = new PersonsDto();
            var persons = await personService.GetAll();
            result.Persons = mapper.Map<IEnumerable<PersonDto>>(persons);
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e.InnerException);
            return Problem("Server error occurred", statusCode: 500);
        }
    }
    
    /// <summary>
    /// Gets a person by ID
    /// </summary>
    /// <response code="200">The found persons</response>
    /// <response code="404">Person not found</response>
    /// <response code="500">Server error occurres</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PersonDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<PersonDto>> GetById(int id)
    {
        try
        {
            var person = await personService.GetById(id);
            var result = mapper.Map<PersonDto>(person);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message, e.InnerException);
            return NotFound("Person not found");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e.InnerException);
            return Problem("Server error occurred", statusCode: 500);
        }
    }
    
    /// <summary>
    /// Creates a person
    /// </summary>
    /// <response code="200">A person is created</response>
    /// <response code="500">Server error occurres</response>
    [HttpPost("create")]
    [ProducesResponseType(typeof(PersonDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<PersonDto>> Create(PersonDto dto)
    {
        try
        {
            var person = mapper.Map<Person>(dto);
            var result = await personService.Create(person);
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e.InnerException);
            return Problem("Server error occurred", statusCode: 500);
        }
    }
    
    /// <summary>
    /// Updates a person
    /// </summary>
    /// <response code="200">A person is updated</response>
    /// <response code="404">Person not found</response>
    /// <response code="500">Server error occurs</response>
    [HttpPut("update")]
    [ProducesResponseType(typeof(PersonDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<PersonDto>> Update(PersonDto dto)
    {
        try
        {
            var person = mapper.Map<Person>(dto);
            var result = await personService.Update(person);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message, e.InnerException);
            return NotFound("Person not found");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e.InnerException);
            return Problem("Server error occurred", statusCode: 500);
        }
    } 
    
    /// <summary>
    /// Updates a person
    /// </summary>
    /// <response code="200">A person is updated</response>
    /// <response code="404">Person not found</response>
    /// <response code="500">Server error occurs</response>
    [HttpDelete("delete/{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<PersonDto>> Delete(int id)
    {
        try
        {
            await personService.Delete(id);
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message, e.InnerException);
            return NotFound("Person not found");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e.InnerException);
            return Problem("Server error occurred", statusCode: 500);
        }
    }    
}
