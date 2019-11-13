using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace WebhookApp
{
    public interface IUpdateServerStream
    {
        Task SendUpdateAsync(Stream updateStream);
    }

    public class UpdateServerStream : IDisposable, IUpdateServerStream
    {
        private readonly NamedPipeClientStream _pipeClient;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);

        public UpdateServerStream() {
            _pipeClient = new NamedPipeClientStream(".", "telegrambot_upstream",
                PipeDirection.InOut, PipeOptions.Asynchronous);
        }

        public async Task SendUpdateAsync(Stream updateStream) {
            if (!_pipeClient.IsConnected) {
                await _pipeClient.ConnectAsync();
                _pipeClient.ReadMode = PipeTransmissionMode.Message;
            }
            try {
                await semaphoreSlim.WaitAsync();
                await updateStream.CopyToAsync(_pipeClient);
            }
            catch (IOException e) {
                Console.WriteLine($"{e.Message}");
                updateStream.Position = 0;
            }
            finally {
                semaphoreSlim.Release();
            }
        }

        public void Dispose() {
            _pipeClient.Close();
        }
    }
}
