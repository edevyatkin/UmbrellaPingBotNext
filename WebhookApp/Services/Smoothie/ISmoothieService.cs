namespace WebhookApp.Services.Smoothie; 

public interface ISmoothieService {
    int ElapsedCombinations { get; }
    Smoothie BestSmoothie { get; }
    SmoothieStatus BestSmoothieStatus { get; }
    void Filter(Smoothie candidate, SmoothieStatus status);
    Smoothie Peek();
    void Reset();
}
