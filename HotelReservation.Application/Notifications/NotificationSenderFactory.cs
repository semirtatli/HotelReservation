using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Notifications
{
    public class NotificationSenderFactory : INotificationSenderFactory
    {
        private readonly INotificationSender _emailSender;
        private readonly INotificationSender _smsSender;

        public NotificationSenderFactory(
            INotificationSender emailSender,
            INotificationSender smsSender)
        {
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        public INotificationSender Create(NotificationType type) => type switch
        {
            NotificationType.Email => _emailSender,
            NotificationType.Sms => _smsSender,
            _ => throw new ArgumentException($"Unsupported notification type: {type}")
        };
    }
}
