using System.Text.Json;

namespace InventoryApiProject.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;  //Used to call external web APIs

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public async Task<decimal> ConvertEuroToUsd(decimal amount)
        //{
        //    var response = await _httpClient.GetAsync(
        //        "https://open.er-api.com/v6/latest/EUR");

        //    response.EnsureSuccessStatusCode();

        //    var json = await response.Content.ReadAsStringAsync();

        //    using JsonDocument document = JsonDocument.Parse(json);

        //    var usdRate = document
        //        .RootElement
        //        .GetProperty("rates")
        //        .GetProperty("USD")
        //        .GetDecimal();

        //    return amount * usdRate;
        //}

        public async Task<decimal> Convert(string from, string to, decimal amount)
        {
            var response = await _httpClient.GetAsync(
                $"https://open.er-api.com/v6/latest/{from}");   //Call External API

            response.EnsureSuccessStatusCode();  //Throw an exception if the response status code is not successful (e.g., 200 OK). This helps to catch errors early and handle them appropriately.

            var json = await response.Content.ReadAsStringAsync();  //read json, Converts HTTP response into string
            using JsonDocument document = JsonDocument.Parse(json);  //Converts string → JSON object

            var rate = document  //Navigate through the JSON structure to extract the exchange rate for the target currency (to) from the "rates" property of the JSON response. The GetDecimal() method is used to convert the extracted value to a decimal type, which is suitable for currency calculations.
                .RootElement
                .GetProperty("rates")
                .GetProperty(to)
                .GetDecimal();

            return amount * rate;  //Calculate the converted amount by multiplying the input amount by the extracted exchange rate and return the result as a decimal value.
        }

    }
}