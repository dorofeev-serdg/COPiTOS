﻿using COPiTOS.Models;

namespace COPiTOS.Repositories;

public interface IPersonRepository
{
    /// <summary>
    /// Gets all persons
    /// </summary>
    /// <returns>All persons</returns>
    Task<IEnumerable<Person>> GetAll();
    
    /// <summary>
    /// Gets a person by ID
    /// </summary>
    /// <param name="id">Person's ID</param>
    /// <returns>Found person</returns>
    Task<Person> GetById(int id);
    
    /// <summary>
    /// Creates a person
    /// </summary>
    /// <param name="person">Person to create</param>
    /// <returns>Saved person</returns>
    Task<Person> Create(Person person);
    
    /// <summary>
    /// Updates a person
    /// </summary>
    /// <param name="person">Person to be updated</param>
    /// <returns>Updated person</returns>
    Task<Person> Update(Person person);
    
    /// <summary>
    /// Deletes a person by ID
    /// </summary>
    /// <param name="id">Person's ID</param>
    Task Delete(int id);
}