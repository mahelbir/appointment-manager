using Application.Features.Appointments.Commands.Busy;
using Application.Features.Appointments.Commands.Cancel;
using Application.Features.Appointments.Commands.Confirm;
using Application.Features.Appointments.Commands.Update;
using Application.Features.Appointments.Queries.GetById;
using Application.Features.Appointments.Queries.GetList;
using Application.Features.Appointments.Queries.GetDetailedCalendar;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;

namespace WebApi.Controllers.AdminControllers;

[Route("api/admin/appointments")]
public class AppointmentsController : BaseController
{

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest request)
    {
        var query = new GetListAppointmentQuery() { PageRequest = request };
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
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateAppointmentCommand command)
    {
        command.Id = id;
        var response = await Mediator.Send(command);
        return Ok(response);
    }
    
    [HttpGet("calendar")]
    public async Task<IActionResult> Calendar([FromQuery] GetDetailedCalendarAppointmentQuery query)
    {
        var response = await Mediator.Send(query);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Busy([FromBody] BusyAppointmentCommand command)
    {
        var response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpPatch("cancel/{id:int}")]
    public async Task<IActionResult> Cancel([FromRoute] int id)
    {
        var command = new CancelAppointmentCommand() { Id = id };
        var response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpPatch("confirm/{id:int}")]
    public async Task<IActionResult> Confirm([FromRoute] int id)
    {
        var command = new ConfirmAppointmentCommand() { Id = id };
        var response = await Mediator.Send(command);
        return Ok(response);
    }
    
}