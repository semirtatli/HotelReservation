using FluentValidation;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.Notifications;
using HotelReservation.Application.Pipeline;
using HotelReservation.Application.Services;
using HotelReservation.Application.Strategies;
using HotelReservation.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

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

            // Chain handler’lar
            services.AddScoped<RoomAvailabilityHandler>();
            services.AddScoped<CapacityCheckHandler>();
            services.AddScoped<PricingCalculationHandler>();

            // Notification
            services.AddScoped<SmsNotificationSender>();
            services.AddScoped<NotificationMessageBuilder>();
            services.AddScoped<IReservationObserver, LoggingObserver>();
            services.AddScoped<IReservationObserver, NotificationObserver>();

            return services;
        }
    }
}
