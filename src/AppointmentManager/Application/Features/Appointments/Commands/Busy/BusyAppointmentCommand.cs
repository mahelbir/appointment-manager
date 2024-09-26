using Application.Features.Appointments.Rules;
using Application.Services.CalendarControlService;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Appointments.Commands.Busy;

public class BusyAppointmentCommand : IRequest<BusyAppointmentResponse>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public class BusyAppointmentCommandHandler : IRequestHandler<BusyAppointmentCommand, BusyAppointmentResponse>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ICalendarControlService _calendarControlService;
        private readonly AppointmentBusinessRules _appointmentBusinessRules;
        private readonly IMapper _mapper;

        public BusyAppointmentCommandHandler(IClientRepository clientRepository, IAppointmentRepository appointmentRepository, ICalendarControlService calendarControlService, AppointmentBusinessRules appointmentBusinessRules, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _appointmentRepository = appointmentRepository;
            _calendarControlService = calendarControlService;
            _appointmentBusinessRules = appointmentBusinessRules;
            _mapper = mapper;
        }

        public async Task<BusyAppointmentResponse> Handle(BusyAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            request.StartDate = request.StartDate.ToUniversalTime();
            request.EndDate = request.EndDate.ToUniversalTime();
            
            await _appointmentBusinessRules.CantOverlap(request.StartDate, request.EndDate);
            
            var appointment = _mapper.Map<Appointment>(request);
            appointment.Client = await _clientRepository.GetAsync(
                predicate: c => c.Contact == "admin@admin.com",
                cancellationToken: cancellationToken
            ) ?? new Client
            {
                FirstName = "Admin",
                LastName = "",
                Contact = "admin@admin.com",
                CreatedDate = DateTime.UtcNow
            };
            appointment.Status = AppointmentStatus.Busy;
            appointment.Client.CreatedDate = DateTime.UtcNow;
            
            appointment = await _calendarControlService.AddCalendarEvent(appointment, cancellationToken);
            
            await _appointmentRepository.AddAsync(appointment, cancellationToken);

            var response = _mapper.Map<BusyAppointmentResponse>(appointment);
            return response;
        }
    }
}