using System.Net.Http.Headers;

namespace AgroSolutions.Ingestion.API.Internal;

public class PropertiesInternalClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public PropertiesInternalClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<string?> GetTalhaoNameAsync(Guid talhaoId, CancellationToken ct)
    {
        var key = _config["InternalApi:Key"];
        using var req = new HttpRequestMessage(HttpMethod.Get, $"/internal/talhoes/{talhaoId}");

        req.Headers.Add("X-Internal-Key", key);

        using var resp = await _http.SendAsync(req, ct);
        if (!resp.IsSuccessStatusCode) return null;

        var json = await resp.Content.ReadAsStringAsync(ct);

        using var doc = System.Text.Json.JsonDocument.Parse(json);
        return doc.RootElement.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null;
    }
}
