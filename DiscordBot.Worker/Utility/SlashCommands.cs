using Discord;
using Discord.WebSocket;

namespace DiscordBot.Worker.Utility;

public static class SlashCommands
{
    /// <summary>
    /// List all slash commands.
    /// </summary>
    /// <returns></returns>
    public static List<SlashCommandBuilder> ToList()
    {
        return [
            // Ping
            new SlashCommandBuilder()
            {
                Name = "ping",
                Description = "Pong!"
            },
            // Guild
            new SlashCommandBuilder()
            {
                Name = "guild",
                Description = "Guild information"
            }
        ];
    }
    
    /// <summary>
    /// Handle for Ping command
    /// </summary>
    /// <param name="command">SocketSlashCommand</param>
    public static async Task PingHandle(SocketSlashCommand command)
    {
        try
        {
            await command.RespondAsync("Pong!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Handle for Guild command
    /// </summary>
    /// <param name="client">DiscordSocketClient</param>
    /// <param name="command">SocketSlashCommand</param>
    /// <exception cref="Exception"></exception>
    public static async Task GuildHandle(DiscordSocketClient client, SocketSlashCommand command)
    {
        try
        {
            var guild = client.GetGuild(command.GuildId ?? throw new Exception("This command is only available for guilds"));
            
            var embedBuilder = new EmbedBuilder()
            {
                Author = new EmbedAuthorBuilder()
                {
                   Name = $"{guild.Name}",
                   IconUrl = guild.IconUrl
                },
                Title = "Guild information",
                Fields = [
                    new EmbedFieldBuilder()
                    {
                        Name = "Name",
                        Value = guild.Name
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Channels",
                        Value = guild.Channels.Count
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Members",
                        Value = guild.MemberCount
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Roles",
                        Value = string.Join(",", guild.Roles.Where(role => !role.IsEveryone).Select(role => role.Mention))
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Created at",
                        Value = guild.CreatedAt
                    },
                ],
                Color = Color.LightGrey,
                ThumbnailUrl = guild.IconUrl,
                Timestamp = DateTimeOffset.Now,
                Footer = new EmbedFooterBuilder()
                {
                    Text = command.User.Username,
                    IconUrl = command.User.GetAvatarUrl()
                }
            };
            
            await command.RespondAsync(embed: embedBuilder.Build());
        }
        catch (Exception e)
        {
            await command.RespondAsync(e.Message);
        }
    }
}