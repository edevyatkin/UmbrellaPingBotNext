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
        private readonly BinaryWriter _binaryWriter;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);

        public UpdateProxy() {
            _pipeClient = new NamedPipeClientStream(".", "telegrambot_upstream",
                PipeDirection.InOut, PipeOptions.Asynchronous);
            _binaryWriter = new BinaryWriter(_pipeClient, Encoding.UTF8);
        }

        public async Task<bool> SendUpdateAsync(string updateJson) {
            if (!_pipeClient.IsConnected) {
                await _pipeClient.ConnectAsync();
            }
            try {
                await semaphoreSlim.WaitAsync();
                _binaryWriter.Write(updateJson);
            }
            catch (Exception e) {
                Console.WriteLine($"{e.Message}");
                return false;
            }
            finally {
                semaphoreSlim.Release();
            }
            return true;
        }

        public void Dispose() {
            _binaryWriter.Close();
        }
    }
}
