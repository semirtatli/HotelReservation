using HotelReservation.Application.Strategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Pipeline
{
    public class PricingCalculationHandler : BaseReservationHandler
    {
        private readonly IPricingStrategy _pricingStrategy;

            public PricingCalculationHandler(IPricingStrategy pricingStrategy)
        {
            _pricingStrategy = pricingStrategy;
        }

        public override async Task HandleAsync(ReservationContext context) {
        
        context.TotalPrice = _pricingStrategy.Calculate(context.Room, context.Request.CheckInDate, context.Request.CheckOutDate);

        await CallNextAsycn(context);
        }
    }
}
