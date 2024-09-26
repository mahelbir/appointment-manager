using Application.Extensions;
using Application.Features.Appointments.Rules;
using Application.Services.AppointmentService;
using Application.Services.CalendarControlService;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.Features.Appointments.Commands.Receive;

public class ReceiveAppointmentCommand : IRequest<ReceivedAppointmentResponse>
{
    public string TokenField => "X-Goog-Channel-Token";
    public string? TokenValue { get; set; }

    public class
        ReceiveAppointmentCommandHandler : IRequestHandler<ReceiveAppointmentCommand, ReceivedAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentService _appointmentService;
        private readonly ICalendarControlService _calendarControlService;
        private readonly AppointmentBusinessRules _appointmentBusinessRules;

        public ReceiveAppointmentCommandHandler(IAppointmentRepository appointmentRepository,
            IAppointmentService appointmentService,
            ICalendarControlService calendarControlService,
            AppointmentBusinessRules appointmentBusinessRules)
        {
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
            _calendarControlService = calendarControlService;
            _appointmentBusinessRules = appointmentBusinessRules;
        }

        public async Task<ReceivedAppointmentResponse> Handle(ReceiveAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            await _calendarControlService.ValidateCalendarToken(request.TokenValue ?? "");
            
            var calendarEvents = await _calendarControlService.GetUpdatedEvents();
            foreach (var calendarEvent in calendarEvents)
            {
                try
                {
                    var appointment = await _appointmentRepository.GetByCalendarEventId(calendarEvent.Id);
                    if (appointment != null)
                    {
                        await UpdateReceivedCalendarEvent(appointment, calendarEvent, cancellationToken);
                    }
                    else
                    {
                        await AddReceivedCalendarEvent(calendarEvent, cancellationToken);
                    }
                }
                catch (Exception)
                {
                }
            }

            var response = new ReceivedAppointmentResponse();
            return response;
        }


        private async Task AddReceivedCalendarEvent(CalendarEvent calendarEvent,
            CancellationToken cancellationToken)
        {
            if (calendarEvent.Status == "cancelled") return;

            await _appointmentBusinessRules.CantOverlap(calendarEvent.StartDate, calendarEvent.EndDate);

            var appointmentStatusProps = _appointmentService
                .GetAppointmentStatusList()
                .Select(s => s.GetProps())
                .First(s => s.ColorId == calendarEvent.Color);
            
            var client = _appointmentService.ParseClientFromDescription(calendarEvent.Description);
            client.CreatedDate = DateTime.UtcNow;
            
            var appointment = new Appointment
            {
                StartDate = calendarEvent.StartDate,
                EndDate = calendarEvent.EndDate,
                Status = appointmentStatusProps.Status,
                CalendarEventId = calendarEvent.Id,
                Client = client
            };

            await _appointmentRepository.AddAsync(appointment, cancellationToken);
        }

        private async Task UpdateReceivedCalendarEvent(Appointment appointment,
            CalendarEvent calendarEvent,
            CancellationToken cancellationToken)
        {
            if (calendarEvent.Status == "cancelled")
            {
                await SetCanceled();
                return;
            }

            var appointmentStatusProps = _appointmentService
                .GetAppointmentStatusList()
                .Select(s => s.GetProps())
                .FirstOrDefault(s => s.ColorId == calendarEvent.Color);

            if (appointmentStatusProps == null || appointmentStatusProps.Status == AppointmentStatus.Cancelled)
            {
                await SetCanceled();
                return;
            }

            await _appointmentBusinessRules.CantOverlap(calendarEvent.StartDate, calendarEvent.EndDate, appointment.Id);

            appointment.StartDate = calendarEvent.StartDate;
            appointment.EndDate = calendarEvent.EndDate;
            appointment.Status = appointmentStatusProps.Status;

            if (appointment.Client != null)
            {
                var client = _appointmentService.ParseClientFromDescription(calendarEvent.Description);
                appointment.Client.FirstName = client.FirstName;
                appointment.Client.LastName = client.LastName;
                appointment.Client.Contact = client.Contact;
                appointment.Client.UpdatedDate = DateTime.UtcNow;
            }

            await _appointmentRepository.UpdateAsync(appointment, cancellationToken);

            return;

            async Task SetCanceled()
            {
                if (appointment.Status != AppointmentStatus.Cancelled)
                {
                    appointment.Status = AppointmentStatus.Cancelled;
                    await _appointmentRepository.UpdateAsync(appointment, cancellationToken);
                }
            }
        }
    }
}