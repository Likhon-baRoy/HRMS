namespace API.DTOs.Salary;

public class CreateSalaryDto
{
  public int EmployeeId { get; set; }

  public decimal BasicSalary { get; set; }

  public decimal HouseRent { get; set; }

  public decimal MedicalAllowance { get; set; }

  public decimal TransportAllowance { get; set; }

  public decimal OtherAllowance { get; set; }

  public DateTime EffectiveDate { get; set; }
}