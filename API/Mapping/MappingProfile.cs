using API.DTOs;
using API.Models;
using AutoMapper;

namespace API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Date flow: Client → DTO → Entity
        CreateMap<Employee, EmployeeDto>();

        // Data flow: Client → DTO → Entity → Database
        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>()
        .ForAllMembers(opts =>
            opts.Condition((src, dest, srcMember) => srcMember != null)); // Ignore null values
    }
}
