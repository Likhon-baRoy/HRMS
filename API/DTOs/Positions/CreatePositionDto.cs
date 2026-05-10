namespace API.DTOs.Positions;

public class CreatePositionDto
{
    public string? Title { get; set; }

    public string? JobLevel { get; set; }

    public int DepartmentId { get; set; }
}
