namespace Application.Features.Clients.Commands.Update;

public class UpdatedClientResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Contact { get; set; }
    public DateTime UpdatedDate { get; set; }
}