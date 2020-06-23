using System.Threading.Tasks;

namespace WebhookApp
{
    public interface IJob
    {
        Task Do();
    }
}
