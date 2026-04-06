using HotelReservation.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Notifications
{
    public class NotificationObserver : IReservationObserver
    {
        private readonly INotificationSenderFactory _senderFactory;
        private readonly NotificationMessageBuilder _messageBuilder;

        public NotificationObserver(
            INotificationSenderFactory senderFactory,
            NotificationMessageBuilder messageBuilder)
        {
            _senderFactory = senderFactory;
            _messageBuilder = messageBuilder;
        }

        public async Task OnReservationCreatedAsync(ReservationCreatedEvent @event)
        {
            var message = _messageBuilder
                .WithRecipient(@event.CustomerEmail)
                .WithSubject("Your reservation has been created")
                .WithReservationDetails(@event)
                .Build();

            var sender = _senderFactory.Create(NotificationType.Email);
            await sender.SendAsync(message);
        }
    }
}