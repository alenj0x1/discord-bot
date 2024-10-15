namespace DiscordBot.Worker.Extensions;

public static class ServicesExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<Worker>();
        services.AddControllers();
    }
}