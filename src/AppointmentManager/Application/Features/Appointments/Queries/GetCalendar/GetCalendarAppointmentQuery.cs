using Application.Features.Appointments.Rules;
using Application.Services.AppointmentService;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Appointments.Queries.GetCalendar;

public class GetCalendarAppointmentQuery : IRequest<IEnumerable<GetCalendarAppointmentListItemDto>>
{
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }

    public class
        CalendarAppointmentQueryQueryHandler : IRequestHandler<GetCalendarAppointmentQuery,
        IEnumerable<GetCalendarAppointmentListItemDto>>
    {
        private readonly IAppointmentService _appointmentService;
        private readonly AppointmentBusinessRules _appointmentBusinessRules;
        private readonly IMapper _mapper;
        
        public CalendarAppointmentQueryQueryHandler(IAppointmentService appointmentService,
            AppointmentBusinessRules appointmentBusinessRules, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _appointmentBusinessRules = appointmentBusinessRules;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetCalendarAppointmentListItemDto>> Handle(
            GetCalendarAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            await _appointmentBusinessRules.DateRangeCantTooLarge(request.StartDate, request.EndDate);
            
            var appointments = await _appointmentService.GetListByDateRange(request.StartDate, request.EndDate);

            var list =
                _mapper.Map<IEnumerable<Appointment>, List<GetCalendarAppointmentListItemDto>>(
                    appointments);
            foreach (var item in list)
            {
                item.Props = _appointmentService.GetAppointmentStatus(item.Status);
            }

            return list;
        }
    }
}