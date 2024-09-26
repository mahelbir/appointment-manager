using Domain.Enums;
using Domain.Models;

namespace Application.Extensions;

public static class AppointmentStatusExtensions
{
    public static AppointmentStatusProps GetProps(this AppointmentStatus appointmentStatus)
    {
        return PropList[appointmentStatus];
    }

    private static readonly Dictionary<AppointmentStatus, AppointmentStatusProps> PropList = new()
    {
        {
            AppointmentStatus.Pending,
            new AppointmentStatusProps
            {
                Status = AppointmentStatus.Pending,
                ColorId = "8",
                ColorCode = "gray",
                Description = "MEŞGUL",
                IsVisible = true
            }
        },
        {
            AppointmentStatus.Confirmed,
            new AppointmentStatusProps
            {
                Status = AppointmentStatus.Confirmed,
                ColorId = "11",
                ColorCode = "red",
                Description = "REZERVE",
                IsVisible = true
            }
        },
        {
            AppointmentStatus.Cancelled,
            new AppointmentStatusProps
            {
                Status = AppointmentStatus.Cancelled,
                ColorId = "9",
                ColorCode = "blue",
                Description = "İPTAL",
                IsVisible = false
            }
        },
        {
            AppointmentStatus.Busy,
            new AppointmentStatusProps
            {
                Status = AppointmentStatus.Busy,
                ColorId = "5",
                ColorCode = "orange",
                Description = "MÜSAİT DEĞİL",
                IsVisible = true
            }
        }
    };
}