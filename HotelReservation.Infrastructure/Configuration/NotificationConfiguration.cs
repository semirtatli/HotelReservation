using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Infrastructure.Configuration
{
    public class NotificationConfiguration
    {
        public string SmtpHost { get; set; } = string.Empty;
        public string SmsApiKey { get; set; } = string.Empty;
        public int MaxRetryCount { get; set; } = 3;
    }
}
