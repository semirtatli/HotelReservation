using HotelReservation.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Notifications.Observers
{
    public interface IReservationObserver
    {
        Task OnReservationCreatedAsync(ReservationCreatedEvent @event);
    }
}
