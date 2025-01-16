using System.Globalization;
using System.Net;
using CryptoScrapper.Persistence.Contexts;
using CryptoScrapper.Persistence.Models;
using HtmlAgilityPack;

namespace CryptoScrapper.Services.AutomationService;

public class CryptoDataService : ICryptoDataService
{
    private readonly ApplicationDbContext _context;
    private const string BaseUrl = "https://www.coingecko.com/en/categories/depin";

    public CryptoDataService(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<int> RunAutomation()
    {
        HttpClient client = new HttpClient(new HttpClientHandler { CookieContainer = new CookieContainer() });
        client.DefaultRequestHeaders.Add("User-Agent", "Your User Agent String");
        client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
        client.DefaultRequestHeaders.Referrer = new Uri(BaseUrl);
        
        // ** Delete all existing coins before fetching new ones **
        _context.Coins.RemoveRange(_context.Coins);
        _context.SaveChanges();

        string html = await client.GetStringAsync(BaseUrl);
        HtmlDocument htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        // This XPath targets the anchor tags that contain coin information
        string xpath = "//a[contains(@class, 'tw-flex') and contains(@class, 'tw-items-center') and contains(@class, 'tw-w-full') and contains(@href, '/en/coins/')]";
        HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes(xpath);

        if (nodes == null)
        {
            Console.WriteLine("No coins found.");
            return 0;
        }

        foreach (HtmlNode coinNode in nodes)
        {
            string coinName = coinNode.SelectSingleNode(".//div[@class='tw-flex tw-flex-col 2lg:tw-flex-row tw-items-start 2lg:tw-items-center']//div[@class='tw-text-gray-700 dark:tw-text-moon-100 tw-font-semibold tw-text-sm tw-leading-5']/text()[1]")?.InnerText.Trim() ?? "Unknown Coin";
            string coinSymbol = coinNode.SelectSingleNode(".//div[@class='tw-block 2lg:tw-inline tw-text-xs tw-leading-4 tw-text-gray-500 dark:tw-text-moon-200 tw-font-medium']").InnerText.Trim();
            
            HtmlNode parentRow = coinNode.ParentNode;
            while (parentRow != null && !parentRow.Name.Equals("tr", StringComparison.OrdinalIgnoreCase))
            {
                parentRow = parentRow.ParentNode;
            }

            if (parentRow != null)
            {
                HtmlNode priceNode = parentRow.SelectSingleNode(".//td[contains(@class, 'tw-text-end')]/span");
                string price = priceNode?.InnerText.Trim() ?? "Price not found";
                
                // Clean up the price string by removing non-numeric characters except for the decimal point
                price = new string(price.Where(c => char.IsDigit(c) || c == '.').ToArray());

                if (decimal.TryParse(price, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal parsedPrice))
                {
                    Console.WriteLine($"Coin: {coinName} - Symbol: {coinSymbol} - Price: {parsedPrice}");
                    var coin = new Coins
                    {
                        Coin = coinName,
                        Symbol = coinSymbol,
                        Price = parsedPrice
                    };
                    _context.Coins.Add(coin);
                    _context.SaveChanges();
                }
                else
                {
                    Console.WriteLine($"Coin: {coinName} - Symbol: {coinSymbol} - Could not parse price: {price}");
                }
            }
            else
            {
                Console.WriteLine($"Coin: {coinName} - Symbol: {coinSymbol} - Price: Price Data Missing");
            }
        }
        
        return 0;
    }
}