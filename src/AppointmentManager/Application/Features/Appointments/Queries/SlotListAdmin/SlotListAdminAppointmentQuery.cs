using Application.Services.AppointmentService;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Appointments.Queries.SlotListAdmin;

public class SlotListAdminAppointmentQuery : IRequest<IEnumerable<SlotListAdminAppointmentItemDto>>
{
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }

    public class
        SlotListAdminAppointmentQueryQueryHandler : IRequestHandler<SlotListAdminAppointmentQuery,
        IEnumerable<SlotListAdminAppointmentItemDto>>
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;


        public SlotListAdminAppointmentQueryQueryHandler(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SlotListAdminAppointmentItemDto>> Handle(
            SlotListAdminAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            var appointments = await _appointmentService.GetListDetailedByDateRange(request.StartDate, request.EndDate);

            var list =
                _mapper.Map<IEnumerable<Appointment>, IEnumerable<SlotListAdminAppointmentItemDto>>(
                    appointments).ToList();
            foreach (var item in list)
            {
                item.Props = _appointmentService.GetAppointmentStatus(item.Status);
            }

            return list;
        }
    }
}