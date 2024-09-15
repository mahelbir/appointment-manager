using Application.Features.Appointments.Rules;
using Application.Services.AppointmentService;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.Appointments.Commands.Confirm;

public class ConfirmAppointmentCommand : IRequest<ConfirmedAppointmentResponse>
{
    public int Id { get; set; }

    public class
        ConfirmAppointmentCommandHandler : IRequestHandler<ConfirmAppointmentCommand, ConfirmedAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentService _appointmentService;
        private readonly AppointmentBusinessRules _appointmentBusinessRules;
        private readonly IMapper _mapper;

        public ConfirmAppointmentCommandHandler(
            IAppointmentRepository appointmentRepository,
            IAppointmentService appointmentService,
            AppointmentBusinessRules appointmentBusinessRules,
            IMapper mapper
        )
        {
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
            _appointmentBusinessRules = appointmentBusinessRules;
            _mapper = mapper;
        }

        public async Task<ConfirmedAppointmentResponse> Handle(ConfirmAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetAsync(
                predicate: a => a.Id == request.Id,
                cancellationToken: cancellationToken
            );

            await _appointmentBusinessRules.ShouldBeExistsWhenSelected(appointment);

            appointment.Status = AppointmentStatus.Confirmed;

            await _appointmentService.UpdateCalendarEventColor(
                appointment,
                cancellationToken
            );

            await _appointmentRepository.UpdateAsync(appointment, cancellationToken);

            var response = _mapper.Map<ConfirmedAppointmentResponse>(appointment);
            return response;
        }
    }
}