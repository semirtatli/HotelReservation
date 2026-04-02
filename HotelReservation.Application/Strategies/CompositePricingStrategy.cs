using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;

namespace HotelReservation.Application.Strategies
{
    public class CompositePricingStrategy : IPricingStrategy
    {
        private readonly IEnumerable<IPricingModifier> _modifiers;

        public CompositePricingStrategy(IEnumerable<IPricingModifier> modifiers)
        {
            _modifiers = modifiers;
        }

        public decimal Calculate(Room room, DateOnly checkIn, DateOnly checkOut)
        {
            decimal basePrice = room.GetPricePerNight() * (decimal)(checkOut.DayNumber - checkIn.DayNumber + 1);
            foreach (var modifier in _modifiers)
            {
                basePrice *= modifier.GetMultiplier(checkIn, checkOut);
            }
            return basePrice;
        }
    }
}
