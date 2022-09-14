namespace WebhookApp.Services.Laboratory; 

public interface ILaboratoryService {
    public int CalculateSalary(int level, int workers);
}