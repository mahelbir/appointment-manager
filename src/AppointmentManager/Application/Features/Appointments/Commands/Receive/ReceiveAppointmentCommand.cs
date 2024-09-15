using Application.Features.Appointments.Rules;
using Application.Services.AppointmentService;
using Application.Services.CalendarControlService;
using Application.Services.Repositories;
using MediatR;

namespace Application.Features.Appointments.Commands.Receive;

public class ReceiveAppointmentCommand : IRequest<ReceivedAppointmentResponse>
{
    public string TokenField => "X-Goog-Channel-Token";
    public string? TokenValue { get; set; }

    public class
        ReceiveAppointmentCommandHandler : IRequestHandler<ReceiveAppointmentCommand, ReceivedAppointmentResponse>
    {
        private readonly ICalendarControlService _calendarControlService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentService _appointmentService;
        private readonly AppointmentBusinessRules _appointmentBusinessRules;

        public ReceiveAppointmentCommandHandler(ICalendarControlService calendarControlService,
            IAppointmentRepository appointmentRepository, IAppointmentService appointmentService,
            AppointmentBusinessRules appointmentBusinessRules)
        {
            _calendarControlService = calendarControlService;
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
            _appointmentBusinessRules = appointmentBusinessRules;
        }

        public async Task<ReceivedAppointmentResponse> Handle(ReceiveAppointmentCommand request,
            CancellationToken cancellationToken)
        {

            await _calendarControlService.ValidateCalendarToken(request.TokenValue ?? "");

            await _calendarControlService.ReceiveCalendarEvents(cancellationToken);

            return new ReceivedAppointmentResponse();
        }
    }
}