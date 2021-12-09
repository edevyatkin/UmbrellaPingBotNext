using System.Collections.Generic;

namespace WebhookApp.Services.Laboratory; 

public interface ILaboratoryService {
    public int CalculateSalary(int level, int workers);
    public int CalculateSalary(int level, IEnumerable<int> workersList);
}