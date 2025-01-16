using CryptoScrapper.Services.CoinDataService;
using Microsoft.AspNetCore.Mvc;

namespace CryptoScrapper.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoinDataController : Controller
{
    private readonly ICoinService _coinService;

    public CoinDataController(ICoinService coinService)
    {
        _coinService = coinService;
    }
    [HttpGet]
    public IActionResult Get()
    {
        var coins = _coinService.GetAllCoins();
        return Ok(coins);
    }
}