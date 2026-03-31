using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.DTO
{
    public class HotelResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
