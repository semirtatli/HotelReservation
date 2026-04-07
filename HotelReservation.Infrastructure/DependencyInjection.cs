using HotelReservation.Application.Configuration;
using HotelReservation.Domain.RepositoryInterfaces;
using HotelReservation.Infrastructure.Data;
using HotelReservation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservation.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Repository’ler
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();

            // Singleton config — appsettings’ten bir kez okunur
            services.AddSingleton(
                configuration.GetSection("Notifications").Get<NotificationConfiguration>()!);

            return services;
        }
    }
}
