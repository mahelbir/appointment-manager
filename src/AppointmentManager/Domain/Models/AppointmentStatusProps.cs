using Domain.Enums;

namespace Domain.Models;

public class AppointmentStatusProps
{
    public string Description { get; set; }
    public bool IsVisible { get; set; }
    public string ColorId { get; set; }
    public string ColorCode { get; set; }
    public AppointmentStatus Status { get; set; }

    public int StatusId => (int)Status;
    public string StatusCode => Status.ToString();
}