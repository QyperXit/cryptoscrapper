using CryptoScrapper.Services.AutomationService;
using Microsoft.AspNetCore.Mvc;

namespace CryptoScrapper.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CryptoDataServiceController : ControllerBase
{
    private readonly ICryptoDataService _cryptoDataService;

    public CryptoDataServiceController(ICryptoDataService cryptoDataService)
    {
        _cryptoDataService = cryptoDataService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        try
        {
            var result = await _cryptoDataService.ScrapeCoinMarket();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}