using DiscordBot.Worker.Extensions;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.MapControllers();

await app.RunAsync();