using FluentValidation;
using HotelReservation.Application.DTO;

namespace HotelReservation.Application.Validators
{
    public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
    {
        public UpdateCustomerRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.DateOfBirth).NotEqual(default(DateOnly));
        }
    }
}
