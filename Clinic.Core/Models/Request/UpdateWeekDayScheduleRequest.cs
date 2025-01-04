namespace Clinic.Core.Models.Request;

public class UpdateWeekDayScheduleRequest
{
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public TimeOnly? BreakStartTime { get; set; }
    public TimeOnly? BreakEndTime { get; set; }
}
