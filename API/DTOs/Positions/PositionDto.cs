namespace API.DTOs.Positions;

public class PositionDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string JobLevel { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;
}
