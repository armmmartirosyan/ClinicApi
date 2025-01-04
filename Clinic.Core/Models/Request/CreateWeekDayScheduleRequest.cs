namespace Clinic.Core.Models.Request;

public class CreateWeekDayScheduleRequest
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public TimeOnly BreakStartTime { get; set; }
    public TimeOnly BreakEndTime { get; set; }
    public long DoctorId { get; set; }
    public int WeekDayId { get; set; }
}
