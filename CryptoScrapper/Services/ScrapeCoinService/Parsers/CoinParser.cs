// CoinParser.cs
using HtmlAgilityPack;
using System.Globalization;

namespace CryptoScrapper.Services.ScrapeCoinService.Parsers;

public class CoinParser
{
    public (string name, string symbol, decimal? price) ParseCoinData(HtmlNode coinNode)
    {
        var name = ParseCoinName(coinNode);
        var symbol = ParseCoinSymbol(coinNode);
        var price = ParseCoinPrice(coinNode);
        return (name, symbol, price);
    }

    private string ParseCoinName(HtmlNode node) =>
        node.SelectSingleNode(".//div[@class='tw-flex tw-flex-col 2lg:tw-flex-row tw-items-start 2lg:tw-items-center']//div[@class='tw-text-gray-700 dark:tw-text-moon-100 tw-font-semibold tw-text-sm tw-leading-5']/text()[1]")
            ?.InnerText.Trim() ?? "Unknown Coin";

    private string ParseCoinSymbol(HtmlNode node) =>
        node.SelectSingleNode(".//div[@class='tw-block 2lg:tw-inline tw-text-xs tw-leading-4 tw-text-gray-500 dark:tw-text-moon-200 tw-font-medium']")
            ?.InnerText.Trim() ?? "Unknown Symbol";

    private decimal? ParseCoinPrice(HtmlNode coinNode)
    {
        var parentRow = FindParentRow(coinNode);
        if (parentRow == null) return null;

        var priceNode = parentRow.SelectSingleNode(".//td[contains(@class, 'tw-text-end')]/span");
        var priceText = priceNode?.InnerText.Trim() ?? "0";
        var cleanPrice = new string(priceText.Where(c => char.IsDigit(c) || c == '.').ToArray());

        return decimal.TryParse(cleanPrice, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var price) 
            ? price 
            : null;
    }

    private HtmlNode FindParentRow(HtmlNode node)
    {
        while (node != null && !node.Name.Equals("tr", StringComparison.OrdinalIgnoreCase))
        {
            node = node.ParentNode;
        }
        return node;
    }
}