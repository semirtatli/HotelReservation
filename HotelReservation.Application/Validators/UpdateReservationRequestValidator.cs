using FluentValidation;
using HotelReservation.Application.DTO;

namespace HotelReservation.Application.Validators
{
    public class UpdateReservationRequestValidator : AbstractValidator<UpdateReservationRequest>
    {
        public UpdateReservationRequestValidator()
        {
            RuleFor(x => x.NumberOfGuests).GreaterThan(0);
            RuleFor(x => x.CheckOutDate).GreaterThanOrEqualTo(x => x.CheckInDate);
        }
    }
}
