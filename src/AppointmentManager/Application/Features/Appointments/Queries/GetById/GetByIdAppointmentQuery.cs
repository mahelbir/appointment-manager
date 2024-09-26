using Application.Features.Appointments.Rules;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.Appointments.Queries.GetById;

public class GetByIdAppointmentQuery : IRequest<GetByIdAppointmentResponse>
{
    public required int Id { get; set; }

    public class GetByIdAppointmentQueryHandler : IRequestHandler<GetByIdAppointmentQuery, GetByIdAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly AppointmentBusinessRules _appointmentBusinessRules;
        private readonly IMapper _mapper;

        public GetByIdAppointmentQueryHandler(IAppointmentRepository appointmentRepository,
            AppointmentBusinessRules appointmentBusinessRules, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _appointmentBusinessRules = appointmentBusinessRules;
            _mapper = mapper;
        }

        public async Task<GetByIdAppointmentResponse> Handle(GetByIdAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            var appointment = await _appointmentBusinessRules.ShouldBeExistId(request.Id);

            var response = _mapper.Map<GetByIdAppointmentResponse>(appointment);
            return response;
        }
    }
}