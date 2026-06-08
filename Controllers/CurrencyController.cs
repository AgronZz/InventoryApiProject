using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InventoryApiProject.Services;
using InventoryApiProject.Dtos;

namespace InventoryApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(
            ICurrencyService currencyService,
            ILogger<CurrencyController> logger)
        {
            _currencyService = currencyService;
            _logger = logger;
        }


        [HttpGet("convert")]
        public async Task<IActionResult> Convert(string from, string to, decimal amount = 1)
        {
            if (amount <= 0)
                return BadRequest("Amount must be greater than 0");  //prevents invalid amounts from being processed

            var result = await _currencyService.Convert(from, to, amount);  //calls the method from service to perform currency conversion

            //return Ok(new
            //{
            //    From = from,
            //    To = to,
            //    Amount = amount,
            //    Result = result
            //});
            var dto = new CurrencyConversionDto  //creates a new instance of the CurrencyConversionDto class and populates its properties with the values from the request and the conversion result
            {
                From = from,
                To = to,
                Amount = amount,
                Result = result
            };

            return Ok(dto);  //returns the CurrencyConversionDto object as the response to the client, which will be serialized to JSON format by ASP.NET Core's built-in JSON serializer
        }
    }
}