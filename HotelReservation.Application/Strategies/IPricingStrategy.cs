using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Strategies
{
    public interface IPricingStrategy
    {
        decimal Calculate(Room room, DateOnly checkIn, DateOnly checkOut);
    }
}
