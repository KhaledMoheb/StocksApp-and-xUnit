using Service;
using ServiceContract;
using StockApp.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
string? finnhubToken = builder.Configuration["FinnhubToken"];
builder.Services.AddScoped<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new FinnhubCompanyProfileService(httpClientFactory, configuration, finnhubToken);
});
builder.Services.AddScoped<IFinnhubStockPriceQuoteService, FinnhubStockPriceQuoteService>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new FinnhubStockPriceQuoteService(httpClientFactory, configuration, finnhubToken);
});
builder.Services.AddScoped<IStocksService, StocksService>();

builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection(nameof(TradingOptions)));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
