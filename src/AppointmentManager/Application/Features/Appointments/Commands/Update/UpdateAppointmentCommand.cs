using Application.Features.Appointments.Rules;
using Application.Services.CalendarControlService;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.Appointments.Commands.Update;

public class UpdateAppointmentCommand : IRequest<UpdatedAppointmentResponse>
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, UpdatedAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ICalendarControlService _calendarControlService;
        private readonly AppointmentBusinessRules _appointmentBusinessRules;
        private readonly IMapper _mapper;

        public UpdateAppointmentCommandHandler(IAppointmentRepository appointmentRepository,
            ICalendarControlService calendarControlService, AppointmentBusinessRules appointmentBusinessRules,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _calendarControlService = calendarControlService;
            _appointmentBusinessRules = appointmentBusinessRules;
            _mapper = mapper;
        }

        public async Task<UpdatedAppointmentResponse> Handle(UpdateAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            request.StartDate = request.StartDate.ToUniversalTime();
            request.EndDate = request.EndDate.ToUniversalTime();
            
            var appointment = await _appointmentBusinessRules.ShouldBeExistId(request.Id);
            appointment.StartDate = appointment.StartDate.ToUniversalTime();
            appointment.EndDate = appointment.EndDate.ToUniversalTime();
            _appointmentBusinessRules.CantLessTime(request.StartDate, request.EndDate, appointment.StartDate,
                appointment.EndDate);
            await _appointmentBusinessRules.CantOverlap(request.StartDate, request.EndDate, appointment.Id);

            appointment = _mapper.Map(request, appointment);

            appointment = await _calendarControlService.UpdateCalendarEvent(appointment, cancellationToken);

            await _appointmentRepository.UpdateAsync(appointment, cancellationToken);

            var response = _mapper.Map<UpdatedAppointmentResponse>(appointment);
            return response;
        }
    }
}