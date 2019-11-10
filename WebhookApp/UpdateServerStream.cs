using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WebhookApp
{
    public interface IUpdateServerStream
    {
        Task SendUpdateAsync(Update update);
    }

    public class UpdateServerStream : IDisposable, IUpdateServerStream
    {
        private readonly NamedPipeClientStream _pipeClient;
        private readonly StreamWriter _textWriter;
        private readonly JsonSerializer _jsonSerializer;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);

        public UpdateServerStream() {
            _pipeClient = new NamedPipeClientStream(".", "telegrambot_upstream", PipeDirection.Out);
            _textWriter = new StreamWriter(_pipeClient);
            _jsonSerializer = JsonSerializer.CreateDefault();
        }

        public async Task SendUpdateAsync(Update update) {
            if (!_pipeClient.IsConnected)
                await _pipeClient.ConnectAsync();
            try {
                await semaphoreSlim.WaitAsync();
                _jsonSerializer.Serialize(_textWriter, update);
                await _textWriter.FlushAsync();
            }
            finally {
                semaphoreSlim.Release();
            }

        }

        public void Dispose() {
           _textWriter.Dispose();
        }
    }
}