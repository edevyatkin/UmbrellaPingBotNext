using System.Collections.Generic;
using System.Linq;

namespace WebhookApp.Services.Laboratory;

class LaboratoryService : ILaboratoryService {
    public int CalculateSalary(int level, int workers) => (20 * level + level * (workers - 1)) * workers / 2;

    public int CalculateSalary(int level, IEnumerable<int> workersList) =>
        workersList.Select(n => CalculateSalary(level, n)).Sum();
}