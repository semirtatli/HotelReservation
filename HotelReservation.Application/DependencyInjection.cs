using Microsoft.Extensions.DependencyInjection;
using HotelReservation.Application.Interfaces;
using HotelReservation.Application.Services;

namespace HotelReservation.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            

            // Service’ler
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IRoomService, RoomService>();

            return services;
        }
    }
}
