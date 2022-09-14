using Moq;
using WebhookApp.Services.Laboratory;
using Xunit;

namespace WebhookApp.Tests;

public class LaboratoryServiceTests
{
    private readonly ILaboratoryService _service;
    
    public LaboratoryServiceTests()
    {
        _service = Mock.Of<LaboratoryService>();
    }
    
    [Theory]
    [InlineData(65, 15, 16575)]
    [InlineData(67, 25, 36850)]
    [InlineData(59, 20, 23010)]
    public void TestCorrectSalaryCalculation(int level, int workers, int salary)
    {
        Assert.Equal(salary, _service.CalculateSalary(level, workers));
    }
}