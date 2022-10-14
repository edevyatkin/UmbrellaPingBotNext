namespace WebhookApp.Services.Smoothie; 

public interface ISmoothieService {
    int ElapsedCombinations { get; }
    Smoothie BestSmoothie { get; }
    SmoothieStatus BestSmoothieStatus { get; }
    string BestSmoothieDescription { get; }
    void Filter(Smoothie candidate, SmoothieStatus status);
    Smoothie Peek();
    void Reset();
}
