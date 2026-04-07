using HotelReservation.Application.Notifications.Models;
using HotelReservation.Application.Notifications.Senders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Notifications.Decorators
{
    public class LoggingNotificationDecorator : INotificationSender
    {
        private readonly INotificationSender _inner;
        private readonly ILogger<LoggingNotificationDecorator> _logger;

        public LoggingNotificationDecorator(INotificationSender inner, ILogger<LoggingNotificationDecorator> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task SendAsync(NotificationMessage message) {

            _logger.LogInformation("Sending notification to {To}", message.To);
            await _inner.SendAsync(message);
            _logger.LogInformation("Notification sent to {To}", message.To);
        }
    }
}
