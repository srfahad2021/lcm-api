using System.Numerics;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args
});

// Disable reloadOnChange to avoid Linux inotify file watcher limits on Render
builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
builder.Configuration.AddEnvironmentVariables();

// 1. Register CORS services
builder.Services.AddCors();

var app = builder.Build();

// 2. Enable CORS globally for all domains
app.UseCors(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.MapGet("/app/shojiburfahad_gmail_com", (string? x, string? y) =>
{
    if (!IsNaturalNumber(x, out BigInteger a) || !IsNaturalNumber(y, out BigInteger b))
        return Results.Text("NaN", "text/plain");

    BigInteger gcd = BigInteger.GreatestCommonDivisor(a, b);
    BigInteger lcm = (a / gcd) * b;

    return Results.Text(lcm.ToString(), "text/plain");
});

static bool IsNaturalNumber(string? value, out BigInteger number)
{
    number = BigInteger.Zero;
    if (string.IsNullOrEmpty(value))
        return false;

    if (!value.All(char.IsDigit))
        return false;

    return BigInteger.TryParse(value, out number) && number > 0;
}

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");