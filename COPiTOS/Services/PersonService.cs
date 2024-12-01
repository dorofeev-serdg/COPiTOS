using COPiTOS.Models;
using COPiTOS.Repositories;

namespace COPiTOS.Services;

public class PersonService(IPersonRepository personRepository, ILogger<PersonService> logger)
    : IPersonService
{
    /// <summary>
    /// Gets all persons
    /// </summary>
    /// <returns>All persons</returns>
    public async Task<IEnumerable<Person>> GetAll()
    {
        return await personRepository.GetAll();
    }

    /// <summary>
    /// Gets a person by ID
    /// </summary>
    /// <param name="id">Person's ID</param>
    /// <returns>Found person</returns>
    /// <exception cref="KeyNotFoundException">When a person not found</exception>
    public async Task<Person?> GetById(int id)
    {
        return await personRepository.GetById(id);
    }

    /// <summary>
    /// Creates a person
    /// </summary>
    /// <param name="person">Person to save</param>
    /// <returns>Saved person</returns>
    public async Task<Person> Create(Person person)
    {
        return await personRepository.Create(person);
    }

    /// <summary>
    /// Updates a person
    /// </summary>
    /// <param name="person">Person to be updated</param>
    /// <returns>Updated person</returns>
    public async Task<Person> Update(Person person)
    {
        return await personRepository.Update(person);
    }

    /// <summary>
    /// Deletes a person by ID
    /// </summary>
    /// <param name="id">Person's ID</param>
    /// <exception cref="KeyNotFoundException">When a person's by ID not found</exception>
    public async Task Delete(int id)
    {
        await personRepository.Delete(id);
    }
}
