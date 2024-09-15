using Application.Features.Appointments.Commands.Book;
using Application.Features.Appointments.Commands.Receive;
using Application.Features.Appointments.Queries.Calendar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace WebApi.Controllers;

[Route("api/appointments")]
public class AppointmentsController : BaseController
{
    [HttpGet("calendar")]
    public async Task<IActionResult> Calendar([FromQuery] CalendarAppointmentQuery query)
    {
        var response = await Mediator.Send(query);
        return Ok(response);
    }
    
    [HttpPost("book")]
    public async Task<IActionResult> Book([FromBody] BookAppointmentCommand command)
    {
        var response = await Mediator.Send(command);
        return Ok(response);
    }
    
    [HttpPost("receive-calendar-updates")]
    public async Task<IActionResult> ReceiveCalendarUpdates()
    {
        var command = new ReceiveAppointmentCommand();
        Request.Headers.TryGetValue(command.TokenField, out StringValues token); 
        command.TokenValue = token;
        var response = await Mediator.Send(command);
        return Ok(response);
    }
    
}