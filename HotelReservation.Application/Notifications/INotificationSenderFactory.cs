using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Notifications
{
    public interface INotificationSenderFactory
    {
        INotificationSender Create(NotificationType type);
    }
}
