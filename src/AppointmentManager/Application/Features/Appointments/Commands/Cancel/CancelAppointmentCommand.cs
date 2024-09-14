using Application.Features.Appointments.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.Appointments.Commands.Cancel;

public class CancelAppointmentCommand : IRequest<CanceledAppointmentResponse>
{
    public int Id { get; set; }

    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, CanceledAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly AppointmentBusinessRules _appointmentBusinessRules;
        private readonly IMapper _mapper;

        public CancelAppointmentCommandHandler(IAppointmentRepository appointmentRepository,
            AppointmentBusinessRules appointmentBusinessRules, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _appointmentBusinessRules = appointmentBusinessRules;
            _mapper = mapper;
        }

        public async Task<CanceledAppointmentResponse> Handle(CancelAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetAsync(
                predicate: a => a.Id == request.Id,
                cancellationToken: cancellationToken
            );

            await _appointmentBusinessRules.ShouldBeExistsWhenSelected(appointment);

            appointment.Status = AppointmentStatus.Cancelled;
            
            await _appointmentRepository.UpdateAsync(appointment, cancellationToken);

            var response = _mapper.Map<CanceledAppointmentResponse>(appointment);
            return response;
        }
    }
}