﻿using System;
using System.Collections.ObjectModel;
using Discord;
using Discord.WebSocket;
using Noob.API.Models;

namespace Noob.API.Test.Stub
{
    public class CommandOptionStub : IApplicationCommandInteractionDataOption
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public ApplicationCommandOptionType Type { get; set; }
        public IList<IApplicationCommandInteractionDataOption> _Options { get; set; }
        public IReadOnlyCollection<IApplicationCommandInteractionDataOption> Options =>
            new ReadOnlyCollection<IApplicationCommandInteractionDataOption>(_Options);

        public CommandOptionStub()
        {

        }

        public CommandOptionStub(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }

    public class DiscordInteractionDataStub : IDiscordInteractionData, IApplicationCommandInteractionData
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public IList<IApplicationCommandInteractionDataOption> _Options { get; set; }
        public IReadOnlyCollection<IApplicationCommandInteractionDataOption> Options =>
            new ReadOnlyCollection<IApplicationCommandInteractionDataOption>(_Options);

        public DiscordInteractionDataStub()
        {

        }

        public DiscordInteractionDataStub(IEnumerable<(string, object)> options) =>
            _Options = options
                .Select(o => new CommandOptionStub(o.Item1, o.Item2))
                .ToList<IApplicationCommandInteractionDataOption>();
    }

    public class InteractionStub : ISlashCommandInteraction
    {
        public ulong Id { get; set; }
        public InteractionType Type { get; set; }
        public string Token { get; set; }
        public int Version { get; set; }
        public bool HasResponded { get; set; }
        public IUser User { get; set; }
        public string UserLocale { get; set; }
        public string GuildLocale { get; set; }
        public bool IsDMInteraction { get; set; }
        public ulong? ChannelId { get; set; }
        public ulong? GuildId { get; set; }
        public ulong ApplicationId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public DiscordInteractionDataStub _Data { get; set; }
        public IApplicationCommandInteractionData Data => _Data;
        IDiscordInteractionData IDiscordInteraction.Data => _Data;

        public InteractionStub()
        {

        }

        public InteractionStub(IUser user)
        {
            User = user;
        }

        public InteractionStub(IUser user, IEnumerable<(string, object)> options)
        {
            User = user;
            _Data = new DiscordInteractionDataStub(options);
        }

        public Task DeferAsync(bool ephemeral = false, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOriginalResponseAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IUserMessage> FollowupAsync(string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IUserMessage> FollowupWithFilesAsync(IEnumerable<FileAttachment> attachments, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IUserMessage> GetOriginalResponseAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IUserMessage> ModifyOriginalResponseAsync(Action<MessageProperties> func, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task RespondAsync(string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task RespondWithFilesAsync(IEnumerable<FileAttachment> attachments, string text = null, Embed[] embeds = null, bool isTTS = false, bool ephemeral = false, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed embed = null, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task RespondWithModalAsync(Modal modal, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }
    }
}

