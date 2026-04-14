using System;
using System.Net;
using System.Net.Http;
using Hangfire;
using Hangfire.LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using WebhookApp;
using WebhookApp.Data;
using WebhookApp.Rules;
using WebhookApp.Services;
using WebhookApp.Services.Battle;
using WebhookApp.Services.Job;
using WebhookApp.Services.Laboratory;
using WebhookApp.Services.LongPolling;
using WebhookApp.Services.Lottery;
using WebhookApp.Services.Ping;
using WebhookApp.Services.Smoothie;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<BotConfig>()
    .BindConfiguration(nameof(BotConfig))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<BotConfig>>().Value);
builder.Services.AddSingleton<ISmoothieService, SmoothieService>();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IBattleService, BattleService>();
builder.Services.AddScoped<ILotteryService, LotteryService>();
builder.Services.AddScoped<IPingService, PingService>();
builder.Services.Scan(scan => {
    scan.FromAssemblyOf<IUpdateRule>()
        .AddClasses(classes => classes.AssignableTo<IUpdateRule>())
        .AsSelfWithInterfaces()
        .WithScopedLifetime();
});
builder.Services.AddScoped<UpdateHandler>();
builder.Services.AddTransient<ILaboratoryService, LaboratoryService>();

var botConfig = builder.Configuration.GetSection(nameof(BotConfig)).Get<BotConfig>();

builder.Services.AddHttpClient("tgbot")
    .RemoveAllLoggers()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        if (!string.IsNullOrEmpty(botConfig.ProxyHost))
        {
            return new SocketsHttpHandler
            {
                Proxy = new WebProxy(botConfig.ProxyHost), 
                UseProxy = true
            };
        }

        return new SocketsHttpHandler();
    })
    .AddTypedClient<ITelegramBotClient>(client => new TelegramBotClient(botConfig.Token, client));

if (string.IsNullOrEmpty(botConfig.WebhookUrl))
{
    builder.Services.AddScoped<ReceiverService>();
    builder.Services.AddHostedService<PollingService>();
}
else
{
    builder.Services.AddControllers();
}

builder.Services.AddHangfire(configuration => {
    configuration
        .UseLiteDbStorage();
});
builder.Services.AddHangfireServer(options =>
    options.SchedulePollingInterval = TimeSpan.FromSeconds(5));
builder.Services.AddSingleton<JobService>();

var app = builder.Build();
var botClient = app.Services.GetRequiredService<ITelegramBotClient>();

if (!string.IsNullOrEmpty(botConfig.WebhookUrl))
{
    await botClient.SetWebhook(botConfig.WebhookUrl);
    app.UseRouting();
    app.UseCors();
    app.MapControllers();
}
else
{
    await botClient.DeleteWebhook();
}

var jobService = app.Services.GetRequiredService<JobService>();
jobService.AddAllJobs();

await app.RunAsync();
