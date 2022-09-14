using System.Collections.Generic;
using System.Linq;

namespace WebhookApp.Services.Laboratory;

public static class LaboratoryServiceExtensions {
    public static int CalculateSalary(this ILaboratoryService service, int level, IEnumerable<int> workersList) =>
        workersList.Sum(n => service.CalculateSalary(level, n));
}
