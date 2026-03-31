using HotelReservation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.DTO
{
    public class CreateHotelRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
