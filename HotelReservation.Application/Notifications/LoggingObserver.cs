using HotelReservation.Domain.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Application.Notifications
{
    public class LoggingObserver : IReservationObserver
    {
        private readonly ILogger<LoggingObserver> _logger;

        public LoggingObserver(ILogger<LoggingObserver> logger)
        {
            _logger = logger;
        }

        public Task OnReservationCreatedAsync(ReservationCreatedEvent @event)
        {
            _logger.LogInformation(
             "Reservation created — Id: {ReservationId}, CustomerId: {CustomerId}, " +
             "CheckIn: {CheckInDate}, CheckOut: {CheckOutDate}",
             @event.ReservationId,
             @event.CustomerId,
             @event.CheckInDate,
             @event.CheckOutDate);

            return Task.CompletedTask;
        }
    }
}
