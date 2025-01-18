using CryptoScrapper.Persistence.Models;
using CryptoScrapper.Services.CoinDataService;
using CryptoScrapper.Services.CoinDataService.DTOS;
using CryptoScrapper.Services.ScrapeCoinService.Factories;
using CryptoScrapper.Services.ScrapeCoinService.Parsers;
using HtmlAgilityPack;

namespace CryptoScrapper.Services.ScrapeCoinService;

public class CryptoDataService : ICryptoDataService
{
    private readonly ICryptoHttpClientFactory httpClientFactory;
    private readonly CoinParser coinParser;
    private readonly ICoinService coinService;
    private const string BaseUrl = "https://www.coingecko.com/en/categories/depin";

    public CryptoDataService(
        ICryptoHttpClientFactory httpClientFactory,
        CoinParser coinParser,
        ICoinService coinService)
    {
        this.httpClientFactory = httpClientFactory;
        this.coinParser = coinParser;
        this.coinService = coinService;
    }

    public async Task<int> ScrapeCoinMarket()
    {
        try
        {
            await coinService.ClearAllCoins();
            var html = await FetchHtmlContent();
            var coins = await ExtractCoinsFromHtml(html);
            return coins.Count;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in scraping: {ex.Message}");
            return 0;
        }
    }

    private async Task<string> FetchHtmlContent()
    {
        using var client = httpClientFactory.CreateClient(BaseUrl);
        return await client.GetStringAsync(BaseUrl);
    }

    private async Task<List<Coins>> ExtractCoinsFromHtml(string html)
    {
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var xpath = "//a[contains(@class, 'tw-flex') and contains(@class, 'tw-items-center') and contains(@class, 'tw-w-full') and contains(@href, '/en/coins/')]";
        var nodes = htmlDocument.DocumentNode.SelectNodes(xpath);

        var coins = new List<Coins>();
        
        if (nodes == null)
        {
            Console.WriteLine("No coins found.");
            return coins;
        }

        foreach (var node in nodes)
        {
            var (name, symbol, price) = coinParser.ParseCoinData(node);
            
            if (price.HasValue)
            {
                var coinDto = new CoinRequesDTO
                {
                    Coin = name,
                    Symbol = symbol,
                    Price = price.Value
                };
                
                var coin = coinService.CreateCoin(coinDto);
                coins.Add(coin);
            }
        }

        return coins;
    }
}