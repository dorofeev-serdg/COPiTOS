using COPiTOS.Models;
using COPiTOS.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace COPiTOS_UnitTests.Repositories;

[TestFixture]
public class PersonRepositoryTests
{
    private PersonListRepository _personListRepository; 
    
    [SetUp]
    public void Setup()
    {
        var loggerMock = new Mock<ILogger<PersonListRepository>>();
        _personListRepository = new PersonListRepository(loggerMock.Object);
    }
   
    [Test]
    public async Task Create_ReturnsPerson()
    {
        int maxId = 0;
        var savedPersons = await _personListRepository.GetAll();
        if (savedPersons.Any())
        {
            maxId = savedPersons.Max(x => x.Id);
        }
        
        var person = CreatePerson(null, "Mr", "FirstName", "LastName", -10000, "DE", "12345", "Musterstadt", "Hauptstrasse 1");
        var result = await _personListRepository.Create(person);
        ValidatePerson(result, maxId + 1, "Mr", "FirstName", "LastName", DateTime.Now.Date.AddDays(-10000), "DE", "12345", "Musterstadt", "Hauptstrasse 1");
    } 
    
    [Test]
    public async Task Update_ReturnsPerson()
    {
        var person = CreatePerson(null, "Mr", "FirstName", "LastName", -10000, "DE", "12345", "Musterstadt", "Hauptstrasse 1");
        var createdPerson = await _personListRepository.Create(person);
        var updateData = CreatePerson(createdPerson.Id, "Mr1", "FirstName1", "LastName1", -20000, "AA", "09876", "Musterstadt1", "Hauptstrasse 12");
        var updatedPerson = await _personListRepository.Update(updateData);
        ValidatePerson(updatedPerson, createdPerson.Id, "Mr1", "FirstName1", "LastName1", DateTime.Now.Date.AddDays(-20000), "AA", "09876", "Musterstadt1", "Hauptstrasse 12");
    } 
     
    [Test]
    public async Task GetAll_ReturnsAllPersons()
    {
        var person = CreatePerson(null, "Mr", "FirstName", "LastName", -10000, "DE", "12345", "Musterstadt", "Hauptstrasse 1");
        var createdPerson = await _personListRepository.Create(person);
        var persons = await _personListRepository.GetAll();
        persons.Count().Should().BeGreaterThan(1);
        persons.FirstOrDefault(x=> x.Id == createdPerson.Id).Should().NotBeNull();
    } 
    
    [Test]
    public async Task GetById_ReturnsPerson()
    {
        var person = CreatePerson(null, "Mr", "FirstName", "LastName", -10000, "DE", "12345", "Musterstadt", "Hauptstrasse 1");
        var createdPerson = await _personListRepository.Create(person);
        var restoredPerson = await _personListRepository.GetById(createdPerson.Id);
        ValidatePerson(restoredPerson, createdPerson.Id, "Mr", "FirstName", "LastName", DateTime.Now.Date.AddDays(-10000), "DE", "12345", "Musterstadt", "Hauptstrasse 1");
    } 
    
    [Test]
    public async Task GetById_WithInvalidId_ThrowsError()
    {
        var person = CreatePerson(null, "Mr", "FirstName", "LastName", -10000, "DE", "12345", "Musterstadt", "Hauptstrasse 1");
        var createdPerson = await _personListRepository.Create(person);
        var action = async () => { await _personListRepository.GetById(createdPerson.Id + 1000); };
        await action.Should().ThrowAsync<Exception>();
    } 
    
    [Test]
    public async Task Delete_RemovesPerson()
    {
        var person = CreatePerson(null, "Mr", "FirstName", "LastName", -10000, "DE", "12345", "Musterstadt", "Hauptstrasse 1");
        var createdPerson = await _personListRepository.Create(person);
        await _personListRepository.Delete(createdPerson.Id);
        var persons = await _personListRepository.GetAll();
        persons.FirstOrDefault(x=> x.Id == createdPerson.Id).Should().BeNull();
    } 
    
    [Test]
    public async Task Delete_WithInvalidId_ThrowsError()
    {
        var person = CreatePerson(null, "Mr", "FirstName", "LastName", -10000, "DE", "12345", "Musterstadt", "Hauptstrasse 1");
        var createdPerson = await _personListRepository.Create(person);
        var action = async () => { await _personListRepository.Delete(createdPerson.Id + 1000); };
        await action.Should().ThrowAsync<Exception>();
    } 
    
    [Test]
    public async Task Update_WithInvalidId_ThrowsError()
    {
        var person = CreatePerson(null, "Mr", "FirstName", "LastName", -10000, "DE", "12345", "Musterstadt", "Hauptstrasse 1");
        var createdPerson = await _personListRepository.Create(person);
        var updateData = CreatePerson(createdPerson.Id + 1000, "Mr", "FirstName", "LastName", -10000, "DE", "12345", "Musterstadt", "Hauptstrasse 1");
        var action = async () => { await _personListRepository.Update(updateData); };
        await action.Should().ThrowAsync<Exception>();
    } 
    
    private Person CreatePerson(int? id, string title, string firstName, string lastName,
        int? dateDifference, string state, string index, string city, string address)
    {
        var person = new Person();
        if (id != null)
        {
            person.Id = id.Value;
        }
        person.Title = title;
        person.FirstName = firstName;
        person.LastName = lastName;
        if(dateDifference != null) {
            person.BirthDate = DateTime.Today.AddDays(dateDifference.Value);
        }
        person.Index = index;
        person.State = state;
        person.City = city;
        person.Address = address;
        
        return person;
    }

    private void ValidatePerson(Person personDto, int? id, string? title, string? firstName, string? lastName,
        DateTime? dateTime, string? state, string? index, string? city, string? address)
    {
        personDto.Id.Should().Be(id);
        personDto.Title.Should().Be(title);
        personDto.FirstName.Should().Be(firstName);
        personDto.LastName.Should().Be(lastName);
        personDto.BirthDate.Date.Should().Be(dateTime);
        personDto.State.Should().Be(state);
        personDto.City.Should().Be(city);
        personDto.Address.Should().Be(address);
        personDto.Index.Should().Be(index);
    }
}