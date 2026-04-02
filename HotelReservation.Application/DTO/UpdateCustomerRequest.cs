namespace HotelReservation.Application.DTO
{
    public class UpdateCustomerRequest
    {
        public string Name { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
    }
}
