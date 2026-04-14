using System.Threading;
using System.Threading.Tasks;

namespace WebhookApp.Services.LongPolling;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken);
}