using Application.Extensions;
using Application.Features.Appointments.Rules;
using Application.Services.AppointmentService;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Appointments.Queries.GetCalendar;

public class GetCalendarAppointmentQuery : IRequest<IEnumerable<GetCalendarAppointmentListItemDto>>
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public class
        CalendarAppointmentQueryQueryHandler : IRequestHandler<GetCalendarAppointmentQuery,
        IEnumerable<GetCalendarAppointmentListItemDto>>
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly AppointmentBusinessRules _appointmentBusinessRules;
        private readonly IMapper _mapper;

        public CalendarAppointmentQueryQueryHandler(IAppointmentService appointmentService,
            IAppointmentRepository appointmentRepository,
            AppointmentBusinessRules appointmentBusinessRules, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _appointmentRepository = appointmentRepository;
            _appointmentBusinessRules = appointmentBusinessRules;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetCalendarAppointmentListItemDto>> Handle(
            GetCalendarAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            await _appointmentBusinessRules.DateRangeCantTooLarge(request.StartDate, request.EndDate);

            var inStatus = _appointmentRepository.InStatus(_appointmentService.GetVisibleAppointmentStatusList());
            var appointments = await _appointmentRepository.GetListByDateRange(
                inStatus,
                request.StartDate.UtcMin(),
                request.EndDate.UtcMax()
            );

            var response =
                _mapper.Map<IEnumerable<Appointment>, List<GetCalendarAppointmentListItemDto>>(
                    appointments);
            return response;
        }
    }
}