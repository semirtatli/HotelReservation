using HotelReservation.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Interfaces
{
    public interface ICustomerService
    {
        public void AddCustomer(CustomerRequest customerRequest);
    }
}
