using HotelReservation.Application.Configuration;
using HotelReservation.Application.Notifications.Models;
using HotelReservation.Application.Notifications.Senders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Notifications.Decorators
{
    public class RetryNotificationDecorator : INotificationSender
    {
        private readonly INotificationSender _inner;
        private readonly NotificationConfiguration _config;

        public RetryNotificationDecorator(
            INotificationSender inner,
            NotificationConfiguration config)
        {
            _inner = inner;
            _config = config;
        }

        public async Task SendAsync(NotificationMessage message)
        {
            Exception? lastException = null;
            for (int i = 0; i < _config.MaxRetryCount; i++)
            {
                try
                {
                    await _inner.SendAsync(message);
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }
            }
                throw lastException!;
        }

    }
}
