using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Notifications
{
    public class SmsNotificationSender : INotificationSender
    {
        public Task SendAsync(NotificationMessage message) {
            Console.WriteLine($"[SMS] To: {message.To} | {message.Body}");
            return Task.CompletedTask;
        }
    }
}
