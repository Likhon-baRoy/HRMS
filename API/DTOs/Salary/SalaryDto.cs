namespace API.DTOs.Salary;

public class SalaryDto
{
    public int Id
    { get; set; }

    public int EmployeeId
    { get; set; }

    public string EmployeeName
    { get; set; }
        = string.Empty;

    public decimal BasicSalary
    { get; set; }

    public decimal HouseRent
    { get; set; }

    public decimal MedicalAllowance
    { get; set; }

    public decimal TransportAllowance
    { get; set; }

    public decimal OtherAllowance
    { get; set; }

    public decimal GrossSalary
    { get; set; }

    public DateTime EffectiveDate
    { get; set; }

    public bool IsCurrent
    { get; set; }
}