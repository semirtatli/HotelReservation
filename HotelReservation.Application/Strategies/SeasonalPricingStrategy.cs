using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Strategies
{
    public class SeasonalPricingStrategy : IPricingModifier
    {
        public decimal GetMultiplier(DateOnly checkIn, DateOnly checkOut)
        {
            if (checkIn.Month >= 6 && checkIn.Month <= 8)
                return 1.3m;
            return 1.0m;
        }
    }
}
