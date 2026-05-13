using API.Models.Enums;

namespace API.Models;

public abstract class BaseEntity
    : BaseTrackableEntity
{
    public RecordStatus RecordStatus { get; set; } = RecordStatus.Active;
}
