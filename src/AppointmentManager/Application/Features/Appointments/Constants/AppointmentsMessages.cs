namespace Application.Features.Appointments.Constants;

public static class AppointmentsMessages
{
    public const string AppointmentStatusPending = "MEŞGUL";
    public const string AppointmentStatusConfirmed = "REZERVE";
    public const string AppointmentStatusCancelled = "İPTAL";
    public const string AppointmentStatusBusy = "MÜSAİT DEĞİL";
    
    public const string Appointment = "Randevu";
    public const string DontExists = "Randevu mevcut değil";
    public const string PastTime = "Randevu tarihi geçmiş";
    public const string Overlap = "Bu tarih aralığında randevu zaten mevcut";
    public const string DateRangeTooLarge = "Tarih aralığı 1 aydan fazla olamaz";
}