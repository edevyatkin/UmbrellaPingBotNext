namespace WebhookApp.Services.Laboratory;

public class LaboratoryService : ILaboratoryService {
    public int CalculateSalary(int level, int workers) => (20 * level + level * (workers - 1)) * workers / 2;
}