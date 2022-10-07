namespace WebhookApp.Abstractions;

public record User(string Username)
{
    public override string ToString()
    {
        return '@' + Username;
    }
}