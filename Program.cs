using Nest;

var builder = WebApplication.CreateBuilder(args);

// Elasticsearch baðlantý ayarlarý
var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultIndex("web_crawler");

var client = new ElasticClient(settings);

// Elasticsearch istemcisini DI konteynerine ekleyin
builder.Services.AddSingleton<IElasticClient>(client);

// ElasticsearchService'yi DI konteynerine ekleyin
builder.Services.AddSingleton<ElasticsearchService>();

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Middleware to log exceptions
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync($"Internal Server Error: {ex.Message}");
    }
});

app.MapRazorPages();

app.Run();
