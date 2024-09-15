using Application.Features.Appointments.Rules;
using Application.Services.AppointmentService;
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
        private readonly IAppointmentService _appointmentService;
        private readonly AppointmentBusinessRules _appointmentBusinessRules;
        private readonly IMapper _mapper;

        public BookAppointmentCommandHandler(IAppointmentRepository appointmentRepository,
            IAppointmentService appointmentService,
            AppointmentBusinessRules appointmentBusinessRules, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
            _appointmentBusinessRules = appointmentBusinessRules;
            _mapper = mapper;
        }

        public async Task<BookedAppointmentResponse> Handle(BookAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            await _appointmentBusinessRules.CantPastTime(request.StartDate, request.EndDate);
            await _appointmentBusinessRules.CantOverlap(request.StartDate, request.EndDate);

            request.StartDate = request.StartDate.ToUniversalTime();
            request.EndDate = request.EndDate.ToUniversalTime();

            var appointment = _mapper.Map<Appointment>(request);
            appointment.Status = AppointmentStatus.Pending;
            appointment.Client.CreatedDate = DateTime.UtcNow;
            
            appointment = await _appointmentService.AddCalendarEvent(appointment, cancellationToken);

            await _appointmentRepository.AddAsync(appointment, cancellationToken);

            var response = _mapper.Map<BookedAppointmentResponse>(appointment);
            return response;
        }
    }
}