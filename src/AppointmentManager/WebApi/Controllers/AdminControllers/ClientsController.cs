using Application.Features.Clients.Commands.Update;
using Application.Features.Clients.Queries.GetById;
using Application.Features.Clients.Queries.GetList;
using Microsoft.AspNetCore.Mvc;
using NArchitecture.Core.Application.Requests;

namespace WebApi.Controllers.AdminControllers;

[Route("api/admin/clients")]
public class ClientsController: BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest request)
    {
        var query = new GetListClientQuery() { PageRequest = request };
        var response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var query = new GetByIdClientQuery{Id = id};
        var response = await Mediator.Send(query);
        return Ok(response);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateClientCommand command)
    {
        command.Id = id;
        var response = await Mediator.Send(command);
        return Ok(response);
    }
    
}