namespace Clinic.Core.Models.DTO;

public class InfiniteScrollDTO<T>
{
    public bool  AllowNext { get; set; }
    public List<T> Data { get; set; }
}
