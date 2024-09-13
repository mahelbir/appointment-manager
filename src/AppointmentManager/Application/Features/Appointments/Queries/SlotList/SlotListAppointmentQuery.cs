using Application.Services.AppointmentService;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Appointments.Queries.SlotList;

public class SlotListAppointmentQuery : IRequest<IEnumerable<SlotListAppointmentItemDto>>
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public class
        SlotListAppointmentQueryQueryHandler : IRequestHandler<SlotListAppointmentQuery,
        IEnumerable<SlotListAppointmentItemDto>>
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;


        public SlotListAppointmentQueryQueryHandler(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SlotListAppointmentItemDto>> Handle(
            SlotListAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            var appointments = await _appointmentService.GetListByDateRange(request.StartDate, request.EndDate);

            var list =
                _mapper.Map<IEnumerable<Appointment>, IEnumerable<SlotListAppointmentItemDto>>(
                    appointments).ToList();
            foreach (var item in list)
            {
                item.Props = _appointmentService.GetAppointmentStatus(item.Status);
            }

            return list;
        }
    }
}