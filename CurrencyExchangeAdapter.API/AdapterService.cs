using System.Globalization;
using System.Xml;

internal class AdapterService(IHttpClientFactory factory) : IAdapterService
{
    private readonly HttpClient client = factory.CreateClient("tcmb");

    public async Task<IEnumerable<CurrencyModel>> GetCurrenciesAsync()
    {
        var response = await client.GetAsync("");
        response.EnsureSuccessStatusCode();

        string xml = await response.Content.ReadAsStringAsync();
        var doc = new XmlDocument();
        doc.LoadXml(xml);

        var data = doc.SelectNodes("//Currency");
        var currencies = new List<CurrencyModel>();

        foreach (XmlNode item in data!)
        {
            var c = new CurrencyModel(item.Attributes!["CurrencyCode"]!.Value,
                                      item.ChildNodes[2]!.InnerText.Trim(),
                                      toDecimal(item.ChildNodes[3]?.InnerText),
                                      toDecimal(item.ChildNodes[4]?.InnerText),
                                      toDecimal(item.ChildNodes[5]?.InnerText),
                                      toDecimal(item.ChildNodes[6]?.InnerText));
            currencies.Add(c);
        }

        return currencies;
    }

    private decimal? toDecimal(string? text)
    {
        if (decimal.TryParse(text?.Trim(), CultureInfo.InvariantCulture, out decimal result)) return result;
        return null;
    }
}