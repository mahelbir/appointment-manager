using Application.Features.Appointments.Rules;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Appointments.Commands.Update;

public class UpdateAppointmentCommand : IRequest<UpdatedAppointmentResponse>
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public UpdateAppointmentCommandClient Client { get; set; }

    public class UpdateAppointmentCommandClient
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
    }

    public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, UpdatedAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly AppointmentBusinessRules _appointmentBusinessRules;
        private readonly IMapper _mapper;

        public UpdateAppointmentCommandHandler(IAppointmentRepository appointmentRepository,
            AppointmentBusinessRules appointmentBusinessRules, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _appointmentBusinessRules = appointmentBusinessRules;
            _mapper = mapper;
        }


        public async Task<UpdatedAppointmentResponse> Handle(UpdateAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetAsync(
                include: a => a.Include(a => a.Client),
                predicate: a => a.Id == request.Id,
                cancellationToken: cancellationToken
            );
            
            await _appointmentBusinessRules.ShouldBeExistsWhenSelected(appointment);
            await _appointmentBusinessRules.CantLessTime(request.StartDate, request.EndDate, appointment.StartDate, appointment.EndDate);
            await _appointmentBusinessRules.CantOverlap(request.StartDate, request.EndDate, appointment.Id);

            request.StartDate = request.StartDate.ToUniversalTime();
            request.EndDate = request.EndDate.ToUniversalTime();

            appointment = _mapper.Map(request, appointment);
            appointment.Client.UpdatedDate = DateTime.UtcNow;

            await _appointmentRepository.UpdateAsync(appointment, cancellationToken);

            var response = _mapper.Map<UpdatedAppointmentResponse>(appointment);
            return response;
        }
    }
}