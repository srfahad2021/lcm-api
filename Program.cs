var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/app/shojiburfahad_gmail_com", (string? x, string? y) =>
{
    if (!IsNaturalNumber(x) || !IsNaturalNumber(y))
        return Results.Text("NaN", "text/plain");

    long a = long.Parse(x!);
    long b = long.Parse(y!);

    long gcd = Gcd(a, b);
    long lcm = a / gcd * b;

    return Results.Text(lcm.ToString(), "text/plain");
});

static bool IsNaturalNumber(string? value)
{
    if (string.IsNullOrEmpty(value))
        return false;

    if (!value.All(char.IsDigit))
        return false;

    return long.TryParse(value, out long number) && number > 0;
}

static long Gcd(long a, long b)
{
    while (b != 0)
    {
        long remainder = a % b;
        a = b;
        b = remainder;
    }

    return a;
}

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

app.Run($"http://0.0.0.0:{port}");