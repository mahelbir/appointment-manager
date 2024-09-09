using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;

namespace Application.Features.Appointments.Queries.GetList;

public class GetListAppointmentQuery : IRequest<GetListResponse<GetListAppointmentListItemDto>>
{
    public PageRequest PageRequest { get; set; }

    public class GetListAppointmentQueryHandler : IRequestHandler<GetListAppointmentQuery,
        GetListResponse<GetListAppointmentListItemDto>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public GetListAppointmentQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListAppointmentListItemDto>> Handle(GetListAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            var pageIndex = Math.Max(0, request.PageRequest.PageIndex);
            var pageSize = Math.Min(5, request.PageRequest.PageSize);
            var result = await _appointmentRepository.GetListAsync(
                index: pageIndex,
                size: pageSize,
                cancellationToken: cancellationToken
            );
            var response = _mapper.Map<GetListResponse<GetListAppointmentListItemDto>>(result);
            return response;
        }
    }
}