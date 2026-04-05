using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Notifications
{
    public record NotificationMessage
    (string To, string Subject, string Body);
}
