using COPiTOS.Models;

namespace COPiTOS.Repositories;

public class PersonListRepository(ILogger<PersonListRepository> logger) : IPersonRepository
{
    private static readonly List<Person> Persons = new ();

    /// <summary>
    /// Gets all saved persons
    /// </summary>
    /// <returns>All persons</returns>
    public async Task<IEnumerable<Person>> GetAll()
    {
        return await Task.FromResult(Persons);
    }

    /// <summary>
    /// Gets a person from a list by id
    /// </summary>
    /// <param name="id">Person ID</param>
    /// <returns>Found person</returns>
    /// <exception cref="KeyNotFoundException">When a person not found</exception>
    public async Task<Person> GetById(int id)
    {
        var result = Persons.FirstOrDefault(x => x.Id == id);
        if (result != null) return await Task.FromResult(result);
        logger.LogError($"Person with id {id} not found");
        throw new KeyNotFoundException($"Person with id = {id} not found");

    }

    /// <summary>
    /// Creates a person
    /// </summary>
    /// <param name="person">Person to save</param>
    /// <returns>Saved person</returns>
    public async Task<Person> Create(Person person)
    {
        var maxIndex = Persons.Count > 0 ?  Persons.Max(x => x.Id) : 0;
        person.Id = maxIndex + 1;
        Persons.Add(person);
        return await Task.FromResult(person);
    }

    /// <summary>
    /// Updates a person
    /// </summary>
    /// <param name="person">A person to be updated</param>
    /// <returns>Updated person</returns>
    /// <exception cref="KeyNotFoundException">When a person not found</exception>
    public async Task<Person> Update(Person person)
    {
        var personIndex = Persons.FindIndex(x => x.Id == person.Id);
        if (personIndex == -1)
        {
            logger.LogError($"Person with id {person.Id} not found");
            throw new KeyNotFoundException($"Person with id = {person.Id} not found");
        }
        
        Persons[personIndex].FirstName = person.FirstName;
        Persons[personIndex].LastName = person.LastName;
        Persons[personIndex].Address = person.Address;
        Persons[personIndex].City = person.City;
        Persons[personIndex].State = person.State;
        Persons[personIndex].Title = person.Title;
        Persons[personIndex].BirthDate = person.BirthDate;
        Persons[personIndex].Index = person.Index;
        
        return await Task.FromResult(Persons[personIndex]);
    }
    
    /// <summary>
    /// Deletes a person by ID
    /// </summary>
    /// <param name="id">ID of a person</param>
    /// <exception cref="KeyNotFoundException">When a person's by ID not found</exception>
    public async Task Delete(int id)
    {
        var personIndex = Persons.FindIndex(x => x.Id == id);
        if (personIndex == -1)
        {
            logger.LogError($"Person with id {id} not found");
            throw new KeyNotFoundException($"Person with id = {id} not found");
        }
        Persons.RemoveAt(personIndex);
    }
}
