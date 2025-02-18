using System.Text.Json;

using CleanAuth.Infrastructure.Extensions;

namespace CleanAuth.Infrastructure.Security.Policies.Json;

public class JsonSnakeCasePolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToSnakeCase();
    }
}