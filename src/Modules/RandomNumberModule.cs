﻿using System;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.Configuration;

namespace DiscordRandomNumber.Modules
{
    public class RandomNumberModule : ModuleBase
    {
        private readonly CommandService _service;
        private readonly IConfigurationRoot _config;
        private readonly Random _random;

        public RandomNumberModule(CommandService service, IConfigurationRoot config, Random random)
        {
            _service = service;
            _config = config;
            _random = random;
        }
        
        // ~say hello -> hello
        [Command("say"), Summary("Echos a message.")]
        public async Task Say([Remainder, Summary("The text to echo")]
            string echo)
        {
            // ReplyAsync is a method on ModuleBase
            await ReplyAsync(echo);
        }

        /// <summary>
        /// Usage: random {lower limit} {upper limit}
        /// </summary>
        /// <returns>The generated random number</returns>
        [Command("random"), Summary("Generates a random number between two numbers")]
        public async Task GenerateRandomNumber([Summary("The lower limit")] int min,
            [Summary("The upper limit")] int max)
        {
            string generatedNumber;
            try
            {
                generatedNumber = Random.Next(min, max).ToString();
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new Exception("The second argument cannot be smaller than the first.");
            }
            catch (ArgumentException)
            {
                throw new Exception("Please provide a positive number.");
            }
            catch (Exception)
            {
                throw new Exception("Not a number");
            }

            await ReplyAsync(embed: generatedNumber.BuildEmbed());
        }
    }
}