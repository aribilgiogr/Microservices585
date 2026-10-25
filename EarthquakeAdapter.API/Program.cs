var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("koeri", cfg => cfg.BaseAddress = new Uri(builder.Configuration["KOERI"]!));
builder.Services.AddSingleton<IAdapterService, AdapterService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", async (IAdapterService service) => await service.GetEartquakeModelAsync());

app.Run();
