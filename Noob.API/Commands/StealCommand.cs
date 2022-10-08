﻿using System;
using System.ComponentModel;
using Discord;
using Noob.API.Models;
using Noob.API.Repositories;

namespace Noob.API.Commands
{
    public class StealCommand
    {
        private IUserRepository UserRepository;

        public StealCommand(IUserRepository userRepository) =>
            UserRepository = userRepository;

        public async Task Steal(ISlashCommandInteraction command)
        {
            var user = UserRepository.Find(command.User.Id);
            if (user.BrowniePoints <= 0)
                await command.RespondAsync("You need Brownie Points to steal from other players.", ephemeral: true);
            else
                await AttemptSteal(command, user);
        }

        private async Task AttemptSteal(ISlashCommandInteraction command, User user)
        {
            IUser discordTarget = (IUser)command.Data.Options.First().Value;
            var victim = UserRepository.Find(discordTarget.Id);
            if (StealsSuccessfully(user, victim))
                await StealSecretly(command, discordTarget, user, victim);
            else
                await AnnounceTheftFailure(command, discordTarget, user, victim);
        }

        private async Task StealSecretly(ISlashCommandInteraction command, IUser discordTarget, User user, User victim)
        {
            if (victim.Niblets > 0)
                await TransferNiblets(command, discordTarget, user, victim);
            else
                await command.RespondAsync($"{discordTarget.Username} doesn't have any Niblets to steal :(", ephemeral: true);
        }

        private async Task TransferNiblets(ISlashCommandInteraction command, IUser discordTarget, User user, User victim)
        {
            AddExperienceTo(user, CalculateExperience(user, victim));
            RemoveExperienceFrom(victim, CalculateExperience(victim, user));
            var niblets = CalculateNiblets(victim);
            user.Niblets += niblets;
            user.BrowniePoints--;
            victim.Niblets -= niblets;
            UserRepository.Save(user);
            UserRepository.Save(victim);
            await command.RespondAsync($"You stole {NibletTerm(niblets)} from {discordTarget.Username} >:)", ephemeral: true);
        }

        private async Task AnnounceTheftFailure(ISlashCommandInteraction command, IUser discordTarget, User user, User victim)
        {
            RemoveExperienceFrom(user, CalculateExperience(user, victim));
            AddExperienceTo(victim, CalculateExperience(victim, user));
            user.BrowniePoints--;
            UserRepository.Save(user);
            UserRepository.Save(victim);
            await command.RespondAsync($"{command.User.Username} was caught trying to steal from {discordTarget.Username}. What a noob!");
        }

        private long CalculateExperience(User user, User opponent)
        {
            var xp = opponent.Level - user.Level + 5;
            return xp <= 0 ? 0 : xp;
        }

        private int CalculateNiblets(User victim)
        {
            var random = new Random();
            var bonus = random.Next(0, 5);
            var min = random.Next(1, 10) + bonus;
            if (min > victim.Niblets) return victim.Niblets;

            var max = 20 + bonus;
            if (victim.Niblets < max) max = victim.Niblets;

            return random.Next(min, max);
        }

        private void AddExperienceTo(User user, long xp)
        {
            if (xp <= 0) return;
            user.Experience += xp;
        }

        private void RemoveExperienceFrom(User user, long xp)
        {
            if (xp <= 0) return;
            user.Experience -= xp;
            if (user.Experience < 0) user.Experience = 0;
        }

        private bool StealsSuccessfully(User user, User victim)
        {
            var random = new Random();
            var modifier = user.Level - victim.Level;
            var userRoll = random.Next(1, 20) + modifier;
            var victimRoll = random.Next(1, 20);
            return userRoll > victimRoll;
        }

        private string NibletTerm(int niblets) =>
            niblets == 1 ? "1 Niblet" : $"{niblets} Niblets";
    }
}

