using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class Client: Entity<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Contact { get; set; }
    
    public ICollection<Appointment> Appointments { get; set; }

    public Client()
    {
        Appointments = new List<Appointment>();
    }
}