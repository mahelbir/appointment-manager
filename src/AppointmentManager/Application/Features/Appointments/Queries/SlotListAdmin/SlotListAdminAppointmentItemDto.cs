using Domain.Enums;
using Domain.Models;

namespace Application.Features.Appointments.Queries.SlotListAdmin;

public class SlotListAdminAppointmentItemDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public AppointmentStatusProps Props { get; set; }
    public ClientDto Client { get; set; }
    
    public class ClientDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
    }
    
}