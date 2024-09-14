using Application.Features.Appointments.Queries.GetById;
using Application.Features.Appointments.Queries.SlotListAdmin;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.AdminControllers;

[Route("api/admin/appointments")]
public class AppointmentsController : BaseController
{
    [HttpGet("slots")]
    public async Task<IActionResult> Slots([FromQuery] SlotListAdminAppointmentQuery query)
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
    
}