using CryptoScrapper.Persistence.Contexts;
using CryptoScrapper.Persistence.Models;
using CryptoScrapper.Services.CoinDataService;
using CryptoScrapper.Services.CoinDataService.DTOS;
using CryptoScrapper.Services.ScrapeCoinService.Factories;
using CryptoScrapper.Services.ScrapeCoinService.Parsers;
using HtmlAgilityPack;

namespace CryptoScrapper.Services.ScrapeCoinService;

public class CryptoDataService : ICryptoDataService
{
    private readonly ApplicationDbContext _context;
    private readonly ICryptoHttpClientFactory _httpClientFactory;
    private readonly CoinParser _coinParser;
    private readonly ICoinService _coinService;
    private const string BaseUrl = "https://www.coingecko.com/en/categories/depin";

    public CryptoDataService(
        ApplicationDbContext context,
        ICryptoHttpClientFactory httpClientFactory,
        CoinParser coinParser,
        ICoinService coinService)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _coinParser = coinParser;
        _coinService = coinService;
    }

    public async Task<int> ScrapeCoinMarket()
    {
        try
        {
            await ClearExistingCoins();
            var html = await FetchHtmlContent();
            var coins = await ExtractCoinsFromHtml(html);
            await SaveCoins(coins);
            return coins.Count;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in scraping: {ex.Message}");
            return 0;
        }
    }

    private async Task ClearExistingCoins()
    {
    
        await _coinService.ClearAllCoins();

    }

    private async Task<string> FetchHtmlContent()
    {
        using var client = _httpClientFactory.CreateClient(BaseUrl);
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
            var (name, symbol, price) = _coinParser.ParseCoinData(node);
            
            if (price.HasValue)
            {
                // Using CoinService to create coins
                var coinDto = new CoinRequesDTO
                {
                    Coin = name,
                    Symbol = symbol,
                    Price = price.Value
                };
                
                var coin = _coinService.CreateCoin(coinDto);
                coins.Add(coin);
            }
        }

        return coins;
    }

    private async Task SaveCoins(List<Coins> coins)
    {
        // No need to implement this as we're using CoinService to create coins
        await Task.CompletedTask;
    }
}