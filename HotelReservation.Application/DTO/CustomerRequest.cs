namespace HotelReservation.Application.DTO
{
    public class CustomerRequest
    {
        public string Name { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
    }
}
