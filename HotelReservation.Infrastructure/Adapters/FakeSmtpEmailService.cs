using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Infrastructure.Adapters
{
    public class FakeSmtpEmailService : IExternalEmailService
    {
        public Task SendAsync(string to, string subject, string body)
        {
            Console.WriteLine($"[SMTP] To: {to} | Subject: {subject} | Body: {body}");
            return Task.CompletedTask;
        }
    }
}
