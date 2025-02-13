using Newtonsoft.Json.Serialization;

public static class StringSnakeCaseExtension
{
    private static readonly NamingStrategy _snakeCaseNamingStrategy = new SnakeCaseNamingStrategy();

    public static string ToSnakeCase(this string stringToConvert)
    {
        ArgumentNullException.ThrowIfNull(stringToConvert, nameof(stringToConvert));

        return _snakeCaseNamingStrategy.GetPropertyName(stringToConvert, false);
    }
}
