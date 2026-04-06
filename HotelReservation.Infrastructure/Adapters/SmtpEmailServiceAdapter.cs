using HotelReservation.Application.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Infrastructure.Adapters
{
    public class SmtpEmailServiceAdapter : INotificationSender
    {
        private readonly IExternalEmailService _externalService;

        public SmtpEmailServiceAdapter(IExternalEmailService externalService)
        {
            _externalService = externalService;
        }

        public async Task SendAsync(NotificationMessage message)
        {
            await _externalService.SendAsync(message.To, message.Subject, message.Body);
        }
    }
}
