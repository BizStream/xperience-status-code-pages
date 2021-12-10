namespace BizStream.Kentico.Xperience.AspNetCore.StatusCodePages;

public static class StatusCodeCacheKeys
{
    public static string StatusCodePage( int statusCode ) => $"statuscodepage|{statusCode}";
}
