using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace CVEGuard.Library;
public static class OllamaClient
{
    private static readonly HttpClient _httpClient = new HttpClient
    {
        Timeout = TimeSpan.FromMinutes(15)  // Increase timeout to 5 minutes
    };

    public static async Task<string> Post(string prompt, Uri url, string modal)
    {
       
        if (string.IsNullOrWhiteSpace(prompt))
                throw new ArgumentException("Input text cannot be null or empty.", nameof(prompt));

        var requestBody = new
        {
            model = modal,  // Change this to the correct model name if needed
            prompt = prompt,
            stream = false,
            options =new {num_ctx= 64768 }
        };
            
        var promptBody = JsonSerializer.Serialize(requestBody);

        var jsonContent = new StringContent(promptBody, Encoding.UTF8, "application/json");

        using HttpResponseMessage response = await _httpClient.PostAsync(new Uri(url, "/api/generate"), jsonContent);
        response.EnsureSuccessStatusCode();

        var responseStream = await response.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<OllamaResponse>(responseStream);
        var responseSummary = RemoveBeforeThinkTag(responseJson?.response);

        return responseSummary ?? "Nothing generated.";
    }
    public static string RemoveBeforeThinkTag(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        int index = input.IndexOf("</think>", StringComparison.OrdinalIgnoreCase);
        return index >= 0 ? input.Substring(index+("</think>".Length)) : input;
    }

    private class OllamaResponse
    {
        public string? response { get; set; }
    }
}
