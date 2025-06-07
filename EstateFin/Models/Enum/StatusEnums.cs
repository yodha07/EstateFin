namespace EstateFin.Models.Enum.StatusEnums
{
    public enum PaymentStatus
    {
        Pending,
        Success,
        Failed
    }

    public enum BookingStatus
    {
        Pending ,
        Approved,
        Confirmed,
        Rejected,
        Cancelled,
    }

    public enum LeaseStatus
    {
        Pending,
        Active,
        Terminated,
        Accepted,
        Rejected
    }

    public enum RentStatus
    {
        Pending,
        Paid,
        Overdue
    }

    public enum AppointmentStatus
    {
        Pending,
        Accepted,
        Rejected
    }


}






