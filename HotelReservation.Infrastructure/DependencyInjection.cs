using HotelReservation.Application.Configuration;
using HotelReservation.Application.Notifications;
using HotelReservation.Application.RepositoryInterfaces;
using HotelReservation.Infrastructure.Adapters;
using HotelReservation.Infrastructure.Data;
using HotelReservation.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

            // Email altyapısı
            services.AddScoped<IExternalEmailService, FakeSmtpEmailService>();
            services.AddScoped<SmtpEmailServiceAdapter>();

            // Factory — SmtpEmailServiceAdapter Infrastructure’da olduğu için burada kayıt olur
            services.AddScoped<INotificationSenderFactory>(sp =>
            {
                var emailSender = sp.GetRequiredService<SmtpEmailServiceAdapter>();
                var smsSender   = sp.GetRequiredService<SmsNotificationSender>();
                var config      = sp.GetRequiredService<NotificationConfiguration>();
                var logger      = sp.GetRequiredService<ILogger<LoggingNotificationDecorator>>();

                INotificationSender wrappedEmail = new RetryNotificationDecorator(emailSender, config);
                wrappedEmail = new LoggingNotificationDecorator(wrappedEmail, logger);

                INotificationSender wrappedSms = new RetryNotificationDecorator(smsSender, config);
                wrappedSms = new LoggingNotificationDecorator(wrappedSms, logger);

                return new NotificationSenderFactory(wrappedEmail, wrappedSms);
            });

            return services;
        }
    }
}
