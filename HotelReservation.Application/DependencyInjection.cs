using FluentValidation;
using HotelReservation.Application.Interfaces;
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

            services.AddScoped<IPricingStrategy>(_ => new CompositePricingStrategy(
                new IPricingModifier[] { new SeasonalPricingStrategy(), new LongStayDiscountStrategy() }
            ));

            return services;
        }
    }
}
