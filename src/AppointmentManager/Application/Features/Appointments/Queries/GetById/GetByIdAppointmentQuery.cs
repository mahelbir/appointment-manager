using Application.Features.Appointments.Rules;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var result = await _appointmentRepository.GetAsync(
                predicate: a => a.Id == request.Id,
                include: a => a.Include(a => a.Client),
                cancellationToken: cancellationToken
            );

            await _appointmentBusinessRules.ShouldBeExists(result);

            var response = _mapper.Map<GetByIdAppointmentResponse>(result);
            return response;
        }
    }
}