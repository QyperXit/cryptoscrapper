using System.ComponentModel.DataAnnotations;

namespace CryptoScrapper.Persistence.Models;

public class Coins
{
    public int Id { get; set; }
     
    [StringLength(255)] 
    public string Coin { get; set; }
    
    [StringLength(255)] 
    public string Symbol { get; set; }
        
    public decimal Price { get; set; }
}