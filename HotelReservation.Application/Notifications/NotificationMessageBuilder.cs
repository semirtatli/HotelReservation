using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Notifications
{
    public class NotificationMessageBuilder
    {
        private string _to = string.Empty;
        private string _subject = string.Empty;
        private string _body = string.Empty;

        public NotificationMessageBuilder WithRecipient(string to)
        {
            _to = to;
            return this;
        }

        public NotificationMessageBuilder WithSubject(string subject) {
            _subject = subject;
            return this;
        }

        public NotificationMessageBuilder WithReservationDetails(HotelReservation.Domain.Events.ReservationCreatedEvent e)
        {
            _body = $"Your reservation (Id: {e.ReservationId}) " +
                    $"from {e.CheckInDate:d} to {e.CheckOutDate:d} " +
                    $"has been successfully created.";
            return this;
        }

        public NotificationMessage Build() => new(_to, _subject, _body);

    }
}
