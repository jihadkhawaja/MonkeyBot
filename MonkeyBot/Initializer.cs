﻿using AutoMapper;
using AutoMapper.Configuration;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Fclp;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonkeyBot.Common;
using MonkeyBot.Database.Entities;
using MonkeyBot.Services;
using MonkeyBot.Services.Common.Feeds;
using MonkeyBot.Services.Common.GameSubscription;
using MonkeyBot.Services.Common.RoleButtons;
using MonkeyBot.Services.Common.SteamServerQuery;
using MonkeyBot.Services.Common.Trivia;
using MonkeyBot.Services.Implementations;
using NLog.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MonkeyBot
{
    public static class Initializer
    {
        public static async Task InitializeAsync(string[] args)
        {
            var p = new FluentCommandLineParser<ApplicationArguments>();
            p.Setup(arg => arg.BuildDocumentation)
                .As('d', "docu")
                .SetDefault(false)
                .WithDescription("Build the documentation files in the app folder");
            p.SetupHelp("?", "help")
                .Callback(text => Console.WriteLine(text));
            var parseResult = p.Parse(args);
            var parsedArgs = !parseResult.HasErrors ? p.Object : null;

            InitializeMapper();

            var services = ConfigureServices();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            var nlogConfig = SetupNLogConfig();
            loggerFactory.ConfigureNLog(nlogConfig);

            var logger = services.GetService<ILogger<MonkeyClient>>();

            var client = services.GetService<DiscordSocketClient>();
            await client.LoginAsync(TokenType.Bot, (await Configuration.LoadAsync()).ProductiveToken);
            await client.StartAsync();

            var manager = services.GetService<CommandManager>();
            await manager.StartAsync();

            var registry = services.GetService<Registry>();
            JobManager.Initialize(registry);

            var announcements = services.GetService<IAnnouncementService>();
            await announcements.InitializeAsync();

            var gameServerService = services.GetService<IGameServerService>();
            gameServerService.Initialize();

            var gameSubscriptionService = services.GetService<IGameSubscriptionService>();
            gameSubscriptionService.Initialize();

            var roleButtonsService = services.GetService<IRoleButtonService>();
            roleButtonsService.Initialize();

            var feedService = services.GetService<IFeedService>();
            feedService.Start();

            if (parsedArgs != null && parsedArgs.BuildDocumentation)
            {
                await manager.BuildDocumentationAsync(); // Write the documentation
                logger.LogInformation("Documentation built");
            }
        }

        private static NLog.Config.LoggingConfiguration SetupNLogConfig()
        {
            var logConfig = new NLog.Config.LoggingConfiguration();
            var coloredConsoleTarget = new NLog.Targets.ColoredConsoleTarget
            {
                Name = "logconsole",
                Layout = @"${date:format=HH\:mm\:ss} ${logger:shortName=True} | ${message} ${exception}"
            };
            var infoLoggingRule = new NLog.Config.LoggingRule("*", NLog.LogLevel.Info, coloredConsoleTarget);
            logConfig.LoggingRules.Add(infoLoggingRule);
            return logConfig;
        }

        private static void InitializeMapper()
        {
            var cfg = new MapperConfigurationExpression();
            cfg.CreateMap<GuildConfigEntity, GuildConfig>();
            cfg.CreateMap<FeedEntity, FeedDTO>();
            cfg.CreateMap<TriviaScoreEntity, TriviaScore>();
            cfg.CreateMap<GameServerEntity, DiscordGameServerInfo>();
            cfg.CreateMap<GameSubscriptionEntity, GameSubscription>();
            cfg.CreateMap<RoleButtonLinkEntity, RoleButtonLink>();
            Mapper.Initialize(cfg);
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));
            services.AddSingleton(new DbService());
            services.AddSingleton<DiscordSocketClient, MonkeyClient>();
            services.AddSingleton<CommandService, MonkeyCommandService>();
            services.AddSingleton<CommandManager>();
            services.AddSingleton<IAnnouncementService, AnnouncementService>();
            services.AddSingleton<ITriviaService, OTDBTriviaService>();
            services.AddSingleton<IPollService, PollService>();
            services.AddSingleton<IFeedService, FeedService>();
            services.AddSingleton<IGameServerService, GameServerService>();
            services.AddSingleton<IGameSubscriptionService, GameSubscriptionService>();
            services.AddSingleton<IRoleButtonService, RoleButtonService>();
            services.AddSingleton<IChuckService, ChuckService>();
            services.AddSingleton(new Registry());

            var provider = new DefaultServiceProviderFactory().CreateServiceProvider(services);
            return provider;
        }
    }

    public class ApplicationArguments
    {
        public bool BuildDocumentation { get; set; }
    }
}