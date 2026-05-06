using API.Data;
using API.DTOs;
using API.Models;
using API.Requests;
using API.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class EmployeeController : BaseApiController
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public EmployeeController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<IEnumerable<EmployeeDto>>>> GetAll([FromQuery] PaginationParams param)
    {
        var query = _context.Employees.AsQueryable();

        var totalCount = await query.CountAsync();

        var employees = await query
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .ToListAsync();
        
        var data = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        var result = new PagedResult<EmployeeDto>
        {
            Items = data,
            Meta = new PaginationMeta
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)param.PageSize)
            }
        };
        
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<EmployeeDto>>> GetById(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null) return NotFound();

        var data = _mapper.Map<EmployeeDto>(employee);
        
        return Success(data);
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateEmployeeDto dto)
    {
        var employee = _mapper.Map<Models.Employee>(dto);

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, null);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateEmployeeDto dto)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null) return NotFound();

        _mapper.Map(dto, employee);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null) return NotFound();

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
