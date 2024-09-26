using Application.Extensions;
using Application.Services.AppointmentService;
using Application.Services.Repositories;
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
        private readonly IAppointmentRepository _appointmetRepository;
        private readonly IMapper _mapper;

        public CalendarAdminAppointmentQueryQueryHandler(IAppointmentRepository appointmetRepository, IMapper mapper)
        {
            _appointmetRepository = appointmetRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetDetailedCalendarAppointmentListItemDto>> Handle(
            GetDetailedCalendarAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            var appointments = await _appointmetRepository.GetListDetailedByDateRange(
                request.StartDate.UtcMin(),
                request.EndDate.UtcMax()
            );

            var response =
                _mapper.Map<IEnumerable<Appointment>, List<GetDetailedCalendarAppointmentListItemDto>>(appointments);
            return response;
        }
    }
}