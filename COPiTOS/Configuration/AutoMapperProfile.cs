using AutoMapper;
using COPiTOS.DTOs;
using COPiTOS.Models;

namespace COPiTOS.Configuration;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Person, PersonDto>();
        CreateMap<PersonDto, Person>();
    }
}