internal interface IAdapterService
{
	Task<IEnumerable<CurrencyModel>> GetCurrenciesAsync();
}
