namespace InventoryApiProject.Services
{
    public interface ICurrencyService
    {
        //Task<decimal> ConvertEuroToUsd(decimal amount);

        Task<decimal> Convert(string from, string to, decimal amount);
    }
}