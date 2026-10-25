using HtmlAgilityPack;
using System.Globalization;
using System.Text.RegularExpressions;

public class AdapterService(IHttpClientFactory factory) : IAdapterService
{
    private readonly HttpClient client = factory.CreateClient("koeri");
    public async Task<IEnumerable<EartquakeModel>> GetEartquakeModelAsync()
    {
        var response = await client.GetAsync("");
        response.EnsureSuccessStatusCode();

        string html = await response.Content.ReadAsStringAsync();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var pre = doc.DocumentNode.SelectSingleNode("//pre");
        string? preText = pre?.InnerText;
        if (preText != null)
        {
            var earthquakes = new List<EartquakeModel>();

            var lines = preText.Trim().Split('\n').Skip(6);
            foreach (var line in lines)
            {
                var cols = Regex.Split(line, @"\s{2,}");
                var item = new EartquakeModel(DateTime.Parse(cols[0]),
                                              double.Parse(cols[1], CultureInfo.InvariantCulture),
                                              double.Parse(cols[2], CultureInfo.InvariantCulture),
                                              double.Parse(cols[3], CultureInfo.InvariantCulture),
                                              double.Parse(cols[5], CultureInfo.InvariantCulture),
                                              cols[7]);
                earthquakes.Add(item);
            }

            return earthquakes;
        }

        return [];
    }
}