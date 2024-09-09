using Application.Services.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.Appointments.Queries.GetById;

public class GetByIdAppointmentQuery : IRequest<GetByIdAppointmentResponse>
{
    public int Id { get; set; }

    public class GetByIdAppointmentQueryHandler : IRequestHandler<GetByIdAppointmentQuery, GetByIdAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public GetByIdAppointmentQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdAppointmentResponse> Handle(GetByIdAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _appointmentRepository.GetAsync(
                predicate: p => p.Id == request.Id,
                cancellationToken: cancellationToken
            );
            var response = _mapper.Map<GetByIdAppointmentResponse>(result);
            return response;
        }
    }
}