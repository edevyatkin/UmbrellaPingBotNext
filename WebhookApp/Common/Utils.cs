namespace WebhookApp.Common;

public static class Utils
{
    public static string LinkToMessage(long chatId, int messageId)
    {
        return $"https://t.me/c/{chatId.ToString()[4..]}/{messageId.ToString()}";
    }
}