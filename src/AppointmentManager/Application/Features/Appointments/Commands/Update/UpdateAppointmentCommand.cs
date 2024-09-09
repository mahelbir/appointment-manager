using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Appointments.Commands.Update;

public class UpdateAppointmentCommand : IRequest<UpdatedAppointmentResponse>
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AppointmentStatus Status { get; set; }
    
    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, UpdatedAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public UpdateAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<UpdatedAppointmentResponse> Handle(UpdateAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            Appointment? appointment = await _appointmentRepository.GetAsync(
                predicate: p => p.Id == request.Id,
                cancellationToken: cancellationToken
            );
            appointment = _mapper.Map(request, appointment);
            await _appointmentRepository.UpdateAsync(appointment, cancellationToken);
            var response = _mapper.Map<UpdatedAppointmentResponse>(appointment);
            return response;
        }
    }
}