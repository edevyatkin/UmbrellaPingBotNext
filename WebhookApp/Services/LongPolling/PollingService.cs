using System;
using Microsoft.Extensions.Logging;

namespace WebhookApp.Services.LongPolling;

// Compose Polling and ReceiverService implementations
public class PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
    : PollingServiceBase<ReceiverService>(serviceProvider, logger);