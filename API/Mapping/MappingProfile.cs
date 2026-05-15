using API.DTOs;
using API.DTOs.Attendance;
using API.DTOs.Departments;
using API.DTOs.Payroll;
using API.DTOs.Positions;
using API.Extensions;
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
                )
            )
            .ForMember(
                dest => dest.PositionTitle,
                opt => opt.MapFrom(src =>
                    src.Position.Title
                )
            )
            .ForMember(
                dest => dest.EmploymentTypeId,
                opt => opt.MapFrom(src =>
                    (int)src.EmploymentType
                )
            )
            .ForMember(
                dest => dest.EmploymentType,
                opt => opt.MapFrom(src =>
                    src.EmploymentType.GetDisplayName()
                )
            )
            .ForMember(
                dest => dest.EmployeeStatusId,
                opt => opt.MapFrom(src =>
                    (int)src.EmployeeStatus
                )
            )
            .ForMember(
                dest => dest.EmployeeStatus,
                opt => opt.MapFrom(src =>
                    src.EmployeeStatus.GetDisplayName()
                )
            )
            .ForMember(
                dest => dest.RecordStatusId,
                opt => opt.MapFrom(src =>
                    (int)src.RecordStatus
                )
            )
            .ForMember(
                dest => dest.RecordStatus,
                opt => opt.MapFrom(src =>
                    src.RecordStatus.GetDisplayName()
                )
            );

        // Data flow: Client → DTO → Entity → Database
        CreateMap<CreateEmployeeDto, Employee>();

        CreateMap<UpdateEmployeeDto, Employee>()
            .IgnoreNullAndDefaultValues();

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
                ))
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src =>
                    src.RecordStatus.GetDisplayName()
                ));

        CreateMap<CreateDepartmentDto, Department>();

        CreateMap<UpdateDepartmentDto, Department>()
            .IgnoreNullAndDefaultValues();

        // =========================
        // Position
        // =========================

        CreateMap<Position, PositionDto>()
            .ForMember(
                dest => dest.DepartmentName,
                opt => opt.MapFrom(src =>
                    src.Department.Name
                ))
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src =>
                    src.RecordStatus.GetDisplayName()
                ));

        CreateMap<CreatePositionDto, Position>();

        CreateMap<UpdatePositionDto, Position>()
            .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) =>
                    srcMember != null
                ));

        // =========================
        // Attendance
        // =========================

        CreateMap<Attendance, AttendanceDto>()
            .ForMember(
                dest => dest.EmployeeName,
                opt => opt.MapFrom(src =>
                    $"{src.Employee.FirstName} {src.Employee.LastName}"
                )
            )
            .ForMember(
                dest => dest.StatusId,
                opt => opt.MapFrom(src =>
                    (int)src.Status
                )
            )
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src =>
                    src.Status.GetDisplayName()
                )
            );

        // =========================
        // Payroll
        // =========================

        CreateMap<Payroll, PayrollDto>()
            .ForMember(
                dest => dest.EmployeeName,
                opt => opt.MapFrom(src =>
                    $"{src.Employee.FirstName} {src.Employee.LastName}"
                )
            )
            .ForMember(
                dest => dest.StatusId,
                opt => opt.MapFrom(src =>
                    (int)src.Status
                )
            )
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src =>
                    src.Status.GetDisplayName()
                )
            );
    }
}
