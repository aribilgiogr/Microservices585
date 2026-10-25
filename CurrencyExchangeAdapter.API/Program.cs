var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("tcmb", cfg => cfg.BaseAddress = new Uri(builder.Configuration["TCMB"]!));
builder.Services.AddSingleton<IAdapterService, AdapterService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", async (IAdapterService service) => await service.GetCurrenciesAsync());

app.Run();
