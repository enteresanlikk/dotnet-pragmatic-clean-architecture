namespace Bookify.Domain.Shared;

public record Currency
{
    internal static readonly Currency None = new("");
    public static readonly Currency USD = new("USD");
    public static readonly Currency EUR = new("EUR");

    public Currency(string code) => Code = code;

    public string Code { get; init; }

    public static Currency FromCode(string code)
    {
        return All.FirstOrDefault(currency => currency.Code == code) ??
                throw new ApplicationException("The currency code is invalid");
    }

    public static readonly IReadOnlyCollection<Currency> All = new[]
    {
        USD,
        EUR,
    };
}
