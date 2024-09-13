using Application.Features.Appointments.Commands.Create;
using Application.Features.Appointments.Queries.GetById;
using Application.Features.Appointments.Queries.SlotList;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/appointments")]
public class AppointmentsController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> SlotList([FromQuery] SlotListAppointmentQuery query)
    {
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var query = new GetByIdAppointmentQuery { Id = id };
        var response = await Mediator.Send(query);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Booking([FromBody] CreateAppointmentCommand command)
    {
        var response = await Mediator.Send(command);
        return Ok(response);
    }
    
}