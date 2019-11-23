using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebhookApp
{
    public interface IUpdateProxy
    {
        Task<bool> SendUpdateAsync(string updateJson);
    }

    public class UpdateProxy : IDisposable, IUpdateProxy
    {
        private readonly NamedPipeClientStream _pipeClient;

        public UpdateProxy() {
            _pipeClient = new NamedPipeClientStream(".", "telegrambot_upstream",
                PipeDirection.InOut, PipeOptions.Asynchronous);
        }

        public async Task<bool> SendUpdateAsync(string updateJson) {
            if (!_pipeClient.IsConnected) {
                await _pipeClient.ConnectAsync();
            }
            try {
                using var sw = new StreamWriter(_pipeClient, 
                    new UTF8Encoding(false), 4096, true) { AutoFlush = true };
                await sw.WriteLineAsync(updateJson);
            }
            catch (Exception e) {
                Console.WriteLine($"{e.Message}");
                return false;
            }
            return true;
        }

        public void Dispose() {
            _pipeClient.Close();
        }
    }
}
