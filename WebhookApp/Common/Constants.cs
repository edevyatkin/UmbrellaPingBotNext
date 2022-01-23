using System;

namespace WebhookApp.Common; 

public static class Constants {
    public static TimeZoneInfo BotTimeZoneInfo => TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");
}
