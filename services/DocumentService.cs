using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ziurtest.models;

namespace ziurtest.services;

public class DocumentService : IDocumentService {
    private readonly HttpClient client;

    private readonly JsonSerializerOptions options;

    public DocumentService(HttpClient httpClient) {
        client = httpClient;
        var bearerToken = WebAssemblyHostBuilder.CreateDefault().Configuration.GetValue<string>("bearerToken");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",bearerToken);
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<List<DocumentModel>?> GetDocuments() {
        var response = await client.GetAsync("/DocumentosFillsCombos");
        var content = await response.Content.ReadAsStringAsync();

        if(!response.IsSuccessStatusCode) {
            throw new ApplicationException(content);
        }

        return JsonSerializer.Deserialize<List<DocumentModel>>(content, options);
        
    }
}

public interface IDocumentService {
    Task<List<DocumentModel>?> GetDocuments();
}