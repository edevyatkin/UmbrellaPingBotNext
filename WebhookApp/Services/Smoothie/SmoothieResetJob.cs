using System.Threading.Tasks;

namespace WebhookApp.Services.Smoothie;

class SmoothieResetJob : IJob
{
    private readonly BotService _botService;
    private readonly ISmoothieService _smoothieService;

    public SmoothieResetJob(BotService botService, ISmoothieService smoothieService) {
        _botService = botService;
        _smoothieService = smoothieService;
    }
    
    public async Task Do() {
        _smoothieService.Reset();
    }
}
