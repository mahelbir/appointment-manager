using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Appointments.Commands.Delete;

public class DeleteAppointmentCommand: IRequest<DeletedAppointmentResponse>
{
    public int Id { get; set; }
    
    public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, DeletedAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public DeleteAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<DeletedAppointmentResponse> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            Appointment? appointment = await _appointmentRepository.GetAsync(
                predicate: p => p.Id == request.Id,
                cancellationToken: cancellationToken
            );
            
            await _appointmentRepository.DeleteAsync(appointment);

            var response = _mapper.Map<DeletedAppointmentResponse>(appointment);
            return response;
        }
    }
}