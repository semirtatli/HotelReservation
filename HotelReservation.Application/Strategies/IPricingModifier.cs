using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Strategies
{
    public interface IPricingModifier
    {
        public decimal GetMultiplier(DateOnly checkIn, DateOnly checkOut);
    }
}
