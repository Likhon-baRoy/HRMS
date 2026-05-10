using API.DTOs;
using API.DTOs.Departments;
using API.Models;
using AutoMapper;

namespace API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // =========================
        // Employee
        // =========================

        // Date flow: Client → DTO → Entity
        CreateMap<Employee, EmployeeDto>()
            .ForMember(
                dest => dest.DepartmentName,
                opt => opt.MapFrom(src =>
                    src.Department.Name
                ))
            .ForMember(
                dest => dest.PositionTitle,
                opt => opt.MapFrom(src =>
                    src.Position.Title
                ));

        // Data flow: Client → DTO → Entity → Database
        CreateMap<CreateEmployeeDto, Employee>();

        CreateMap<UpdateEmployeeDto, Employee>()
            .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null)); // Ignore null values

        // =========================
        // Department
        // =========================

        CreateMap<Department, DepartmentDto>()
            .ForMember(
                dest => dest.ManagerName,
                opt => opt.MapFrom(src =>
                    src.Manager != null
                        ? $"{src.Manager.FirstName} {src.Manager.LastName}"
                        : null
                ));

        CreateMap<CreateDepartmentDto, Department>();

        CreateMap<UpdateDepartmentDto, Department>()
            .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) =>
                    srcMember != null
                ));
    }
}
