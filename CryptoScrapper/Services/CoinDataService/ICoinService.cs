using CryptoScrapper.Persistence.Models;
using CryptoScrapper.Services.CoinDataService.DTOS;

namespace CryptoScrapper.Services.CoinDataService;

public interface ICoinService
{
    IEnumerable<Coins> GetAllCoins();
    Coins CreateCoin(CoinRequesDTO request);
    Coins UpdateCoin(int id, CoinRequesDTO request);
    bool DeleteCoin(int id);
    Task ClearAllCoins();

}