using System.Net;
using System.Text;
using COPiTOS.DTOs;
using FluentAssertions;
using Newtonsoft.Json;

namespace COPiTOS_IntegrationTests;

[TestFixture]
public class PersonControllerTests: BaseController
{
    [Test]
    public async Task GetAll_AfterInit_ReturnsEmpty()
    {
        var response = await _client.GetAsync("/api/v1/person/all");

        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PersonsDto>(responseContent);

        result.Should().NotBeNull();
        result.Persons.Should().NotBeNull();
    }

    [TestCase(null, "LastName", -10000, "DE","12345", HttpStatusCode.BadRequest)]
    [TestCase("FirstName", null, -10000, "DE","12345", HttpStatusCode.BadRequest)]
    [TestCase("FirstName", "LastName", -10000, "DEZ","12345", HttpStatusCode.BadRequest)]
    [TestCase("FirstName", "LastName", -10000, "D","12345", HttpStatusCode.BadRequest)]
    [TestCase("FirstName", "LastName", -10000, "DE","1234", HttpStatusCode.BadRequest)]
    [TestCase("FirstName", "LastName", -10000, "DE","123456", HttpStatusCode.BadRequest)]
    public async Task Create_WhenDtoInvalid_ReturnsValidCode(string? firstName, string? lastName, 
        int? dateDifference, string? state, string? index, HttpStatusCode expectedCode)
    {
        var personContent = CreatePersonDto(null, null, firstName, lastName, dateDifference,  state, index, null, null );
        
        var responce = await _client.PostAsync("/api/v1/person/create", personContent);
        responce.StatusCode.Should().Be(expectedCode);
    }

    [TestCase(null, "LastName", -10000, "DE","12345", HttpStatusCode.BadRequest)]
    [TestCase("FirstName", null, -10000, "DE","12345", HttpStatusCode.BadRequest)]
    [TestCase("FirstName", "LastName", -10000, "DEZ","12345", HttpStatusCode.BadRequest)]
    [TestCase("FirstName", "LastName", -10000, "D","12345",  HttpStatusCode.BadRequest)]
    [TestCase("FirstName", "LastName", -10000, "DE","1234",  HttpStatusCode.BadRequest)]
    [TestCase("FirstName", "LastName", -10000, "DE","123456", HttpStatusCode.BadRequest)]
    public async Task Update_WhenDtoInvalid_ReturnsValidCode(string? firstName, string? lastName, 
        int? dateDifference, string? state, string? index, HttpStatusCode expectedCode)
    {
        var validDto = CreatePersonDto(null, "Mr", "Ab", "Cd", -10000,  "DE", "12345", null, null);
        var postResponse = await _client.PostAsync("/api/v1/person/create", validDto);
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await postResponse.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PersonDto>(responseContent);
        result.Should().NotBeNull();
        
        var personContent = CreatePersonDto(result.Id, null, firstName, lastName, dateDifference,  state, index, null, null);
        var updateResponse = await _client.PutAsync("/api/v1/person/update", personContent);
        updateResponse.StatusCode.Should().Be(expectedCode);
    }

    [Test]
    public async Task CRUD_WhenDtoValid_ProvidesNoError()
    {
        // Create a person
        var validDto = CreatePersonDto(null, "Mr", "Ab", "Cd", -10000,  "DE", "12345", null, null);
        var postResponse = await _client.PostAsync("/api/v1/person/create", validDto);
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await postResponse.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PersonDto>(responseContent);
        result.Should().NotBeNull();
        
        // Get all persons
        var allPersonsResponse = await _client.GetAsync("/api/v1/person/all");
        allPersonsResponse.Should().NotBeNull();
        allPersonsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await allPersonsResponse.Content.ReadAsStringAsync();
        var personsResult = JsonConvert.DeserializeObject<PersonsDto>(responseContent);
        personsResult.Should().NotBeNull();
        personsResult.Persons.Should().NotBeNull();
        personsResult.Persons.FirstOrDefault(x => x.Id == result.Id).Should().NotBeNull();
        var person = personsResult.Persons.FirstOrDefault(x => x.Id == result.Id);
        
        // Get person by Id
        var personResponse = await _client.GetAsync("/api/v1/person/" + person.Id);
        personResponse.Should().NotBeNull();
        personResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var personResponseContent = await personResponse.Content.ReadAsStringAsync();
        var personResult = JsonConvert.DeserializeObject<PersonDto>(personResponseContent);
        personResult.Should().NotBeNull();
        ValidatePersonDto(personResult, person.Id, "Mr", "Ab", "Cd", DateTime.Today.AddDays(-10000), "DE", "12345", null, null);
        
        // Update a person
        var updatedDto = CreatePersonDto(person.Id, "Mr1", "Ab1", "Cd1", -20000,  "AA", "23456", "Musterstadt", "Haupt Strasse 1");
        var updateResponse = await _client.PutAsync("/api/v1/person/update", updatedDto);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK); 
        
        // Get all persons
        allPersonsResponse = await _client.GetAsync("/api/v1/person/all");
        allPersonsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await allPersonsResponse.Content.ReadAsStringAsync();
        personsResult = JsonConvert.DeserializeObject<PersonsDto>(responseContent);
        personsResult.Should().NotBeNull();
        personsResult.Persons.FirstOrDefault(x => x.Id == result.Id).Should().NotBeNull();
        person = personsResult.Persons.FirstOrDefault(x => x.Id == result.Id);       
        
        // Get updated person
        personResponse = await _client.GetAsync($"/api/v1/person/{person.Id}");
        personResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        personResponseContent = await personResponse.Content.ReadAsStringAsync();
        personResult = JsonConvert.DeserializeObject<PersonDto>(personResponseContent);
        personResult.Should().NotBeNull();
        ValidatePersonDto(personResult, person.Id, "Mr1", "Ab1", "Cd1", DateTime.Today.AddDays(-20000), "AA", "23456", "Musterstadt", "Haupt Strasse 1");

        // Delete the person
        var deleteResponse = await _client.DeleteAsync($"/api/v1/person/delete/{person.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Get all persons
        allPersonsResponse = await _client.GetAsync("/api/v1/person/all");
        allPersonsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent = await allPersonsResponse.Content.ReadAsStringAsync();
        personsResult = JsonConvert.DeserializeObject<PersonsDto>(responseContent);
        personsResult.Persons.FirstOrDefault(x => x.Id == result.Id).Should().BeNull();
    }
    
    private HttpContent CreatePersonDto(int? id, string? title, string? firstName, string? lastName,
        int? dateDifference, string? state, string? index, string? city, string? address)
    {
        var dto = new PersonDto();
        if (id != null)
        {
            dto.Id = id.Value;
        }
        dto.Title = title;
        dto.FirstName = firstName;
        dto.LastName = lastName;
        if(dateDifference != null) {
            dto.BirthDate = DateTime.Today.AddDays(dateDifference.Value);
        }
        dto.Index = index;
        dto.State = state;
        dto.City = city;
        dto.Address = address;
        
        // Let's exclude null fields completely
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.NullValueHandling = NullValueHandling.Ignore;
        var serializedDto = JsonConvert.SerializeObject(dto, settings);
        return new StringContent(serializedDto, Encoding.UTF8, "application/json");
    }

    private void ValidatePersonDto(PersonDto personDto, int? id, string? title, string? firstName, string? lastName,
        DateTime? dateTime, string? state, string? index, string? city, string? address)
    {
        personDto.Id.Should().Be(id);
        personDto.Title.Should().Be(title);
        personDto.FirstName.Should().Be(firstName);
        personDto.LastName.Should().Be(lastName);
        personDto.BirthDate.Value.Date.Should().Be(dateTime);
        personDto.State.Should().Be(state);
        personDto.City.Should().Be(city);
        personDto.Address.Should().Be(address);
        personDto.Index.Should().Be(index);
    }
}