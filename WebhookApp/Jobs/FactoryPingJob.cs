using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using WebhookApp.Services.Factory;
using WebhookApp.Services.Ping;

namespace WebhookApp.Jobs
{
    class FactoryPingJob : IJob
    {
        private readonly ITelegramBotClient _botClient;
        private readonly BotConfig _botConfig;
        private readonly ILogger<FactoryPingJob> _logger;
        private readonly IFactoryService _factoryService;
        private readonly IPingService _pingService;

        public FactoryPingJob(ITelegramBotClient botClient, BotConfig botConfig, ILogger<FactoryPingJob> logger, IFactoryService factoryService, IPingService pingService) {
            _botClient = botClient;
            _botConfig = botConfig;
            _logger = logger;
            _factoryService = factoryService;
            _pingService = pingService;
        }
        
        public async Task Do() {
             foreach (var chatId in _botConfig.Chats) {
                 var usersToPing = await _factoryService.GetUsersToPingAsync(chatId);
                 _logger.LogInformation($"Factory Ping, chatId: {chatId}");
                 await _botClient.SendMessage(
                     chatId: chatId,
                     text: "Началась запись на фабрику!");                
                 await _pingService.PingUsersAsync(usersToPing, chatId);
             }
        }
    }
}
