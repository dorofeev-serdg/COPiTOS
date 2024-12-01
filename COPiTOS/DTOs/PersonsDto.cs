namespace COPiTOS.DTOs;

public class PersonsDto
{
    public IEnumerable<PersonDto> Persons { get; set; } = new List<PersonDto>();
}