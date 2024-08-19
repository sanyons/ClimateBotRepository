using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class NLPService
{
    private readonly HttpClient _httpClient;

    public NLPService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<NLPResponse> GetNLPResponseAsync(string question)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(new { text = question }), System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://localhost:5000/nlp", jsonContent);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<NLPResponse>(jsonResponse);
        }

        throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
    }
}

public class NLPResponse
{
    public string Intent { get; set; }
    public string City { get; set; }
}
