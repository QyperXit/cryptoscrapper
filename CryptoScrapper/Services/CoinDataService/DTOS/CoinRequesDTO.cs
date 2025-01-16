namespace CryptoScrapper.Services.CoinDataService.DTOS;

public class CoinRequesDTO
{
    public string Coin { get; set; }
    
    public string Symbol { get; set; }
        
    public decimal Price { get; set; }
}