using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Infrastructure.Adapters
{
    public interface IExternalEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}
