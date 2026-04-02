using HotelReservation.Application.DTO;
using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Strategies
{
    public class LongStayDiscountStrategy : IPricingModifier
    {
        public decimal GetMultiplier(DateOnly checkIn, DateOnly checkOut)
        {
            if ((checkOut.DayNumber - checkIn.DayNumber + 1) >= 7)
                return 0.9m;
            return 1.0m;
        }
    }
}
