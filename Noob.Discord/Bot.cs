﻿using Discord;
using Discord.WebSocket;
using Noob.DL;
namespace Noob.Discord;

public class Bot
{
    private DiscordSocketClient Client;
    private SlashCommandHandler SlashCommandHandler;

    public Bot(
        IUserRepository userRepository,
        IUserCommandRepository userCommandRepository) =>
        SlashCommandHandler = new SlashCommandHandler(userRepository, userCommandRepository);

    public async Task StartAsync(string token)
    {
        Client = new DiscordSocketClient();
        Client.Log += Log;
        Client.SlashCommandExecuted += SlashCommandHandler.Handle;
        Client.JoinedGuild += SlashCommandHandler.RegisterGuild;
        Client.Ready += ClientReady;
        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();
    }

    private async Task ClientReady()
    {
        foreach (var guild in Client.Guilds)
            await SlashCommandHandler.RegisterGuild(guild);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}