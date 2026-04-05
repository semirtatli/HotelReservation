using HotelReservation.Application.DTO;
using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Pipeline
{
    public class ReservationContext
    {
        public CreateReservationRequest Request { get; set; } = null!;
        public Room Room { get; set; } = null!;
        public decimal TotalPrice { get; set; }
    }
}
