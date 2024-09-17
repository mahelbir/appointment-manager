using Application.Services.CalendarControlService;
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

        public ReceiveAppointmentCommandHandler(ICalendarControlService calendarControlService)
        {
            _calendarControlService = calendarControlService;
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