using HotelReservation.Application.Notifications.Models;

namespace HotelReservation.Application.Notifications.Senders
{
    public class EmailNotificationSender : INotificationSender
    {
        public Task SendAsync(NotificationMessage message)
        {
            Console.WriteLine($"[EMAIL] To: {message.To} | Subject: {message.Subject} | Body: {message.Body}");
            return Task.CompletedTask;
        }
    }
}
