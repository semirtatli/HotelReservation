using HotelReservation.Application.Notifications.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Notifications.Senders
{
    public class SmsNotificationSender : INotificationSender
    {
        public Task SendAsync(NotificationMessage message) {
            Console.WriteLine($"[SMS] To: {message.To} | {message.Body}");
            return Task.CompletedTask;
        }
    }
}
