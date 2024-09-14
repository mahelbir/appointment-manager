using Application.Features.Appointments.Commands.Book;
using Application.Features.Appointments.Queries.Calendar;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpPost("receive-gc-updates")]
    public async Task<IActionResult> ReceiveGcUpdates()
    {
        try
        {
            Request.Headers.TryGetValue("X-Goog-Channel-Token", out var token);
           // await _appointmentService.ReceiveGcEventUpdates(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }
    
}