using FluentValidation;
using HotelReservation.Application.DTO;

namespace HotelReservation.Application.Validators
{
    public class CreateReservationRequestValidator : AbstractValidator<CreateReservationRequest>
    {
        public CreateReservationRequestValidator()
        {
            RuleFor(x => x.RoomId).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.NumberOfGuests).GreaterThan(0);
            RuleFor(x => x.CheckOutDate).GreaterThanOrEqualTo(x => x.CheckInDate);
        }
    }
}
