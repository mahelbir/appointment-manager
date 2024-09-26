using Application.Features.Appointments.Rules;
using Application.Services.CalendarControlService;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Appointments.Commands.Book;

public class BookAppointmentCommand : IRequest<BookedAppointmentResponse>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public BookAppointmentCommandClient Client { get; set; }

    public class BookAppointmentCommandClient
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
    }

    public class BookAppointmentCommandHandler : IRequestHandler<BookAppointmentCommand, BookedAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ICalendarControlService _calendarControlService;
        private readonly AppointmentBusinessRules _appointmentBusinessRules;
        private readonly IMapper _mapper;

        public BookAppointmentCommandHandler(IAppointmentRepository appointmentRepository,
            ICalendarControlService calendarControlService, AppointmentBusinessRules appointmentBusinessRules,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _calendarControlService = calendarControlService;
            _appointmentBusinessRules = appointmentBusinessRules;
            _mapper = mapper;
        }

        public async Task<BookedAppointmentResponse> Handle(BookAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            request.StartDate = request.StartDate.ToUniversalTime();
            request.EndDate = request.EndDate.ToUniversalTime();
            
            _appointmentBusinessRules.CantPastTime(request.StartDate, request.EndDate);
            await _appointmentBusinessRules.CantOverlap(request.StartDate, request.EndDate);

            var appointment = _mapper.Map<Appointment>(request);
            appointment.Status = AppointmentStatus.Pending;
            appointment.Client.CreatedDate = DateTime.UtcNow;

            appointment = await _calendarControlService.AddCalendarEvent(appointment, cancellationToken);

            await _appointmentRepository.AddAsync(appointment, cancellationToken);

            var response = _mapper.Map<BookedAppointmentResponse>(appointment);
            return response;
        }
    }
}