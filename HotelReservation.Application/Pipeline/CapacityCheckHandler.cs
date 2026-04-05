using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Pipeline
{
    public class CapacityCheckHandler : BaseReservationHandler
    {
        public override async Task HandleAsync(ReservationContext context)
        {
            if (context.Request.NumberOfGuests > context.Room.Capacity)
            {
                throw new ArgumentException("The selected room cannot accommodate the number of guests.");
            }
            await CallNextAsync(context);
        }
    }
}
