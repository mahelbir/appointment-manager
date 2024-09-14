using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

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
            var pageSize = request.PageRequest.PageSize > 0 ? Math.Min(100, request.PageRequest.PageSize) : 20;
            var pageIndex = Math.Max(0, request.PageRequest.PageIndex);

            IPaginate<Appointment> users = await _appointmentRepository.GetListAsync(
                index: pageIndex,
                size: pageSize,
                enableTracking: false,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListAppointmentListItemDto> response =
                _mapper.Map<GetListResponse<GetListAppointmentListItemDto>>(users);
            return response;
        }
    }
}