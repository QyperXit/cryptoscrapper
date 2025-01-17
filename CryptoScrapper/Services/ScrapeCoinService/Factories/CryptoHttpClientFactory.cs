using System.Net;

namespace CryptoScrapper.Services.ScrapeCoinService.Factories;

public class CryptoHttpClientFactory : ICryptoHttpClientFactory
{
    public HttpClient CreateClient(string baseUrl)
    {
        var client = new HttpClient(new HttpClientHandler { CookieContainer = new CookieContainer() });
        client.DefaultRequestHeaders.Add("User-Agent", "Your User Agent String");
        client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
        client.DefaultRequestHeaders.Referrer = new Uri(baseUrl);
        return client;
    }
}