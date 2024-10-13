using Discord;
using Discord.WebSocket;
using DiscordBot.Worker.Utility;

namespace DiscordBot.Worker;

public class Worker(ILogger<Worker> logger, IConfiguration configuration) : BackgroundService
{
    private readonly DiscordSocketClient _client = new();
    private readonly ILogger<Worker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                await _client.LoginAsync(TokenType.Bot, configuration["Token"]);
                await _client.StartAsync();

                _client.Ready += OnReady;
                _client.SlashCommandExecuted += SlashCommandsHandler;
                
                await Task.Delay(-1, stoppingToken);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("{message}", e.Message);
            throw;
        }
    }

    private async Task OnReady()
    {
        try
        {

            await _client.SetCustomStatusAsync("my pretty nahito <3");
            await _client.SetStatusAsync(UserStatus.Idle);
            await LoadSlashCommands();
            
            _logger.LogInformation("{BotUsername} is active!", _client.CurrentUser.Username);
        }
        catch (Exception e)
        {
            _logger.LogError("{message}", e.Message);
            throw;
        }
    }
    
    private async Task SlashCommandsHandler(SocketSlashCommand command)
    {
        try
        {
            switch (command.Data.Name)
            {
                case "ping":
                    await SlashCommands.PingHandle(command);
                    break;
                case "guild": 
                    await SlashCommands.GuildHandle(_client, command);
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task LoadSlashCommands()
    {
        try
        {
            var guilds = _client.Guilds.ToList();
            
            foreach (var slashCommand in SlashCommands.ToList())
            {
                foreach (var guild in guilds)
                {
                    try
                    {
                        await _client.Rest.CreateGuildCommand(slashCommand.Build(), guild.Id);
                        _logger.LogInformation("'{SlashCommandName}' loaded on: {GuildId}", slashCommand.Name, guild.Id);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("'{SlashCommandName}' failed to load on: {GuildId}", slashCommand.Name, guild.Id);
                        throw;
                    }
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError("{message}", e.Message);
            throw;
        }
    }
}