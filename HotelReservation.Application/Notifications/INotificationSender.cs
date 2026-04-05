using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Notifications
{
    public interface INotificationSender
    {
        Task SendAsync(NotificationMessage message);
    }
}
