using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly AppDbContext _context;
    public EmployeeController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<ActionResult<Employee>> Create(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetById), new { Id = employee.Id }, employee);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
    {
        var employees = await _context.Employees.ToListAsync();
        return Ok(employees);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetById(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        
        return employee == null ? NotFound() : Ok(employee);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<Employee>> Update(int id, Employee updatedEmployee)
    {
        if (id != updatedEmployee.Id)
            return BadRequest();
            
        var employee = await _context.Employees.FindAsync(id);
        
        if (employee == null)
            return NotFound();
            
        employee.Name = updatedEmployee.Name;
        employee.Email = updatedEmployee.Email;
        employee.Phone = updatedEmployee.Phone;
        employee.Department = updatedEmployee.Department;
        employee.Position = updatedEmployee.Position;
        employee.AccountNumber = updatedEmployee.AccountNumber;
        employee.Status = updatedEmployee.Status;
        
        await _context.SaveChangesAsync();
    
        return Ok(employee);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        
        if (employee == null)
            return NotFound();
            
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}
