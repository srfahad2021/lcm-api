using System.Numerics;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args
});

// Disable reloadOnChange for configuration files to prevent inotify limit crashes
builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false);
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

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