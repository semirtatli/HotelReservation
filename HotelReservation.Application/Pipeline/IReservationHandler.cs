using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Pipeline
{
    public interface IReservationHandler
    {
        IReservationHandler SetNext(IReservationHandler next);
        Task HandleAsync(ReservationContext context);
    }
}
