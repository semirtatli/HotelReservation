using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.DTO
{
    public class CustomerRequest
    {
        public string name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}
