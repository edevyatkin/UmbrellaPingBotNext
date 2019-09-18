using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
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

        public UpdateServerStream() {
            _pipeClient = new NamedPipeClientStream(".", "telegrambot_upstream", PipeDirection.Out);
            _textWriter = new StreamWriter(_pipeClient);
            _jsonSerializer = JsonSerializer.CreateDefault();
        }

        public async Task SendUpdateAsync(Update update) {
            if (!_pipeClient.IsConnected)
                await _pipeClient.ConnectAsync();
            _jsonSerializer.Serialize(_textWriter, update);
            await _textWriter.FlushAsync();
        }

        public void Dispose() {
           _textWriter.Dispose();
        }
    }
}