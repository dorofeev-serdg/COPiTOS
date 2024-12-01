using COPiTOS.Models;
using COPiTOS.Repositories;
using COPiTOS.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace COPiTOS_UnitTests.Services;

[TestFixture]
public class PersonServiceTests
{
    private PersonService _personService;
    private Mock<IPersonRepository> _personRepositoryMock;
    
    [SetUp]
    public void Setup()
    {
        var loggerMock = new Mock<ILogger<PersonService>>();
        _personRepositoryMock = new Mock<IPersonRepository>();
        _personService = new PersonService(_personRepositoryMock.Object, loggerMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsAllPersons()
    {
        _personRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Person>() { new Person() });
        var result = await _personService.GetAll();
        result.Should().NotBeNull();
        result.Count().Should().Be(1);
    }
    
    [Test]
    public async Task GetById_ReturnsPerson()
    {
        _personRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Person{Id = 1});
        var result = await _personService.GetById(1);
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }
    
    [Test]
    public async Task Update_ReturnsPerson()
    {
        _personRepositoryMock.Setup(x => x.Update(It.IsAny<Person>())).ReturnsAsync(new Person {Id = 1});
        var result = await _personService.Update(new Person());
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }
    
    [Test]
    public async Task Create_ReturnsPerson()
    {
        _personRepositoryMock.Setup(x => x.Create(It.IsAny<Person>()));
        var result = await _personService.Create(new Person());
        _personRepositoryMock.Verify(x => x.Create(It.IsAny<Person>()), Times.Once);
    }
}