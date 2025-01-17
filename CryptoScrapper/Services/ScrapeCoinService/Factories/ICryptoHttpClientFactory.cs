namespace CryptoScrapper.Services.ScrapeCoinService.Factories;

public interface ICryptoHttpClientFactory
{
    HttpClient CreateClient(string baseUrl);

}