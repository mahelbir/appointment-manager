using Application.Services.AppointmentService;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Appointments.Queries.GetDetailedCalendar;

public class GetDetailedCalendarAppointmentQuery : IRequest<IEnumerable<GetDetailedCalendarAppointmentListItemDto>>
{
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }

    public class
        CalendarAdminAppointmentQueryQueryHandler : IRequestHandler<GetDetailedCalendarAppointmentQuery,
        IEnumerable<GetDetailedCalendarAppointmentListItemDto>>
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;


        public CalendarAdminAppointmentQueryQueryHandler(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetDetailedCalendarAppointmentListItemDto>> Handle(
            GetDetailedCalendarAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            var appointments = await _appointmentService.GetListDetailedByDateRange(request.StartDate, request.EndDate);

            var list =
                _mapper.Map<IEnumerable<Appointment>, List<GetDetailedCalendarAppointmentListItemDto>>(
                    appointments);
            foreach (var item in list)
            {
                item.Props = _appointmentService.GetAppointmentStatus(item.Status);
            }

            return list;
        }
    }
}