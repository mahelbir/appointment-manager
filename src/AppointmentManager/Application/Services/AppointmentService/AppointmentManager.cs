using System.Text.RegularExpressions;
using Application.Extensions;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.AppointmentService;

public class AppointmentManager : IAppointmentService
{
    public List<AppointmentStatus> GetAppointmentStatusList()
    {
        return Enum
            .GetValues(typeof(AppointmentStatus))
            .Cast<AppointmentStatus>()
            .ToList();
    }
    
    public List<AppointmentStatus> GetVisibleAppointmentStatusList()
    {
        var statuses = GetAppointmentStatusList();
        return statuses
            .Where(s => s.GetProps().IsVisible)
            .ToList();
    }
    
    public Client ParseClientFromDescription(string description)
    {
        description = description.Replace("<br>", "\n");
        var lines = description
            .Split('\n')
            .Select(s => Regex.Replace(s, "<.*?>", String.Empty).Trim())
            .ToArray();

        var nameField = lines[0];
        var names = nameField.Contains(' ') ? nameField.Split(' ', 2) : [nameField, ""];
        var contact = lines.Length > 1 ? lines[1] : "";

        return new Client
        {
            FirstName = names[0],
            LastName = names[1],
            Contact = contact
        };
    }
    
}