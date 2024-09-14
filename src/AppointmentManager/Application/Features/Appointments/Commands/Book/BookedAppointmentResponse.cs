using Domain.Enums;

namespace Application.Features.Appointments.Commands.Book;

public class BookedAppointmentResponse
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public ClientDto Client { get; set; }
    
    public class ClientDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}