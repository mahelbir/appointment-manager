using Application.Features.Appointments.Commands.Create;
using Application.Features.Appointments.Commands.Delete;
using Application.Features.Appointments.Commands.Update;
using Application.Features.Appointments.Queries.GetById;
using Application.Features.Appointments.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;

namespace WebApi.Controllers;

[Route("api/[controller]")]
public class AppointmentsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentCommand command)
    {
        var response = await Mediator.Send(command);
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateAppointmentCommand command)
    {
        var response = await Mediator.Send(command);
        return Ok(response);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id)
    {
        var command = new DeleteAppointmentCommand { Id = id };
        var response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        var query = new GetListAppointmentQuery { PageRequest = pageRequest };
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
}