using FluentValidation;
using HotelReservation.Application.Configuration;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.Notifications.Decorators;
using HotelReservation.Application.Notifications.Models;
using HotelReservation.Application.Notifications.Observers;
using HotelReservation.Application.Notifications.Senders;
using HotelReservation.Application.Pipeline;
using HotelReservation.Application.Services;
using HotelReservation.Application.Strategies;
using HotelReservation.Application.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HotelReservation.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateHotelRequestValidator>();

            // Service’ler
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IRoomService, RoomService>();

            // Pricing strategy
            services.AddScoped<IPricingModifier, SeasonalPricingStrategy>();
            services.AddScoped<IPricingModifier, LongStayDiscountStrategy>();
            services.AddScoped<IPricingStrategy, CompositePricingStrategy>();

            // Chain handler’lar — zincir DI’da kurulur, service sadece IReservationHandler görür
            services.AddScoped<RoomAvailabilityHandler>();
            services.AddScoped<CapacityCheckHandler>();
            services.AddScoped<PricingCalculationHandler>();
            services.AddScoped<IReservationHandler>(sp =>
            {
                var availability = sp.GetRequiredService<RoomAvailabilityHandler>();
                var capacity     = sp.GetRequiredService<CapacityCheckHandler>();
                var pricing      = sp.GetRequiredService<PricingCalculationHandler>();
                availability.SetNext(capacity).SetNext(pricing);
                return availability;
            });

            // Notification senders
            services.AddScoped<EmailNotificationSender>();
            services.AddScoped<SmsNotificationSender>();

            // Factory
            services.AddScoped<INotificationSenderFactory>(sp =>
            {
                var emailSender = sp.GetRequiredService<EmailNotificationSender>();
                var smsSender   = sp.GetRequiredService<SmsNotificationSender>();
                var config      = sp.GetRequiredService<NotificationConfiguration>();
                var logger      = sp.GetRequiredService<ILogger<LoggingNotificationDecorator>>();

                INotificationSender wrappedEmail = new RetryNotificationDecorator(emailSender, config);
                wrappedEmail = new LoggingNotificationDecorator(wrappedEmail, logger);

                INotificationSender wrappedSms = new RetryNotificationDecorator(smsSender, config);
                wrappedSms = new LoggingNotificationDecorator(wrappedSms, logger);

                return new NotificationSenderFactory(wrappedEmail, wrappedSms);
            });

            // Observers
            services.AddScoped<NotificationMessageBuilder>();
            services.AddScoped<IReservationObserver, LoggingObserver>();
            services.AddScoped<IReservationObserver, NotificationObserver>();

            return services;
        }
    }
}
