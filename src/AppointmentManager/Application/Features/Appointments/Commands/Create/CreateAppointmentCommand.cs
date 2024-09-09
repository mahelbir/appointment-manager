using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Appointments.Commands.Create;

public class CreateAppointmentCommand : IRequest<CreatedAppointmentResponse>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; }

    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, CreatedAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public CreateAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }
        
        public async Task<CreatedAppointmentResponse> Handle(CreateAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointment = _mapper.Map<Appointment>(request);
            await _appointmentRepository.AddAsync(appointment, cancellationToken);
            var response = _mapper.Map<CreatedAppointmentResponse>(appointment);
            return response;
        }
    }
}