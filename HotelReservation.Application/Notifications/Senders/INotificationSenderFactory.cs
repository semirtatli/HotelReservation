using HotelReservation.Application.Notifications.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Notifications.Senders
{
    public interface INotificationSenderFactory
    {
        INotificationSender Create(NotificationType type);
    }
}
