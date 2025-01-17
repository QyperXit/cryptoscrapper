using CryptoScrapper.Persistence.Contexts;
using CryptoScrapper.Persistence.Models;
using CryptoScrapper.Services.CoinDataService.DTOS;

namespace CryptoScrapper.Services.CoinDataService;

public class CoinService : ICoinService
{
    private readonly ApplicationDbContext _context;
    
    public CoinService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Coins> GetAllCoins()
    {
        var coins = _context.Coins.ToList();
        return coins;
    }
    
    

    public Coins CreateCoin(CoinRequesDTO request)
    {
        var coin = new Coins
        {
            Coin = request.Coin,
            Symbol = request.Symbol,
            Price = request.Price
            
        };
        _context.Coins.Add(coin);
        _context.SaveChanges();
        return coin;
    }

    public Coins UpdateCoin(int id, CoinRequesDTO request)
    {
        var coin = _context.Coins.Find(id);
        coin.Coin = request.Coin;
        coin.Symbol = request.Symbol;
        coin.Price = request.Price;
        _context.SaveChanges();
        return coin;
    }

    public bool DeleteCoin(int id)
    {
        var coin = _context.Coins.Find(id);
        if (coin != null)
        {
            _context.Coins.Remove(coin);
            _context.SaveChanges();
            return true;
        }
        else
        {
            return false;
            
        }
    }

    public async Task ClearAllCoins()
    {
        _context.Coins.RemoveRange(_context.Coins);
        await _context.SaveChangesAsync();
    }
}