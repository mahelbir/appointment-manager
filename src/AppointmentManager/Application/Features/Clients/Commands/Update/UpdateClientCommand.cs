using Application.Features.Clients.Rules;
using Application.Services.CalendarControlService;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Clients.Commands.Update;

public class UpdateClientCommand : IRequest<UpdatedClientResponse>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Contact { get; set; }

    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, UpdatedClientResponse>
    {
        private readonly IClientRepository _clientRepository;
        private readonly ClientBusinessRules _clientBusinessRules;
        private readonly ICalendarControlService _calendarControlService;
        private readonly IMapper _mapper;

        public UpdateClientCommandHandler(IClientRepository clientRepository,
            ClientBusinessRules clientBusinessRules,
            ICalendarControlService calendarControlService,
            IMapper mapper)
        {
            _clientRepository = clientRepository;
            _clientBusinessRules = clientBusinessRules;
            _calendarControlService = calendarControlService;
            _mapper = mapper;
        }

        public async Task<UpdatedClientResponse> Handle(UpdateClientCommand request,
            CancellationToken cancellationToken)
        {
            await _clientBusinessRules.ShouldBeExistId(request.Id);

            var client = await _clientRepository.GetAsync(
                predicate: c => c.Id == request.Id,
                include: c => c.Include(c => c.Appointments),
                cancellationToken: cancellationToken
            );
            client = _mapper.Map(request, client);

            Task.Run(() =>
            {
                _calendarControlService.UpdateCalendarEventsClient(client.Appointments, client, cancellationToken);
            }, cancellationToken);

            await _clientRepository.UpdateAsync(client, cancellationToken);

            var response = _mapper.Map<UpdatedClientResponse>(client);
            return response;
        }
    }
}