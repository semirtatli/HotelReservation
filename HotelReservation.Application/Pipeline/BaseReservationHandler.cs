using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Pipeline
{
    public abstract class BaseReservationHandler : IReservationHandler
    {
        private IReservationHandler? _next;
        public IReservationHandler SetNext(IReservationHandler next)
        {
            _next = next;
            return next;
        }

        public abstract Task HandleAsync(ReservationContext context);
        protected async Task CallNextAsycn(ReservationContext context)
        {
            if (_next is not null)
            {
                await _next.HandleAsync(context);
            }
        }
    }
}
