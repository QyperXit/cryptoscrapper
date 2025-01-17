namespace CryptoScrapper.Services.ScrapeCoinService;

public interface ICryptoDataService
{
    Task<int> ScrapeCoinMarket();
}