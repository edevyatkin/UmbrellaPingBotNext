using System.Threading.Tasks;

namespace WebhookApp.Services.Smoothie;

class SmoothieResetJob : IJob
{
    private readonly ISmoothieService _smoothieService;

    public SmoothieResetJob(ISmoothieService smoothieService) {
        _smoothieService = smoothieService;
    }
    
    public Task Do() {
        _smoothieService.Reset();
        return Task.CompletedTask;
    }
}
