using Application.Services.AppointmentService;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Appointments.Queries.CalendarAdmin;

public class CalendarAdminAppointmentQuery : IRequest<IEnumerable<CalendarAdminAppointmentItemDto>>
{
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }

    public class
        CalendarAdminAppointmentQueryQueryHandler : IRequestHandler<CalendarAdminAppointmentQuery,
        IEnumerable<CalendarAdminAppointmentItemDto>>
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;


        public CalendarAdminAppointmentQueryQueryHandler(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CalendarAdminAppointmentItemDto>> Handle(
            CalendarAdminAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            var appointments = await _appointmentService.GetListDetailedByDateRange(request.StartDate, request.EndDate);

            var list =
                _mapper.Map<IEnumerable<Appointment>, IEnumerable<CalendarAdminAppointmentItemDto>>(
                    appointments).ToList();
            foreach (var item in list)
            {
                item.Props = _appointmentService.GetAppointmentStatus(item.Status);
            }

            return list;
        }
    }
}