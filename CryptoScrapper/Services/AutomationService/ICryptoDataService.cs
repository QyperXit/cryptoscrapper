namespace CryptoScrapper.Services.AutomationService;

public interface ICryptoDataService
{
    Task<int> RunAutomation();
}