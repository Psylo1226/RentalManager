using System.Net.Http.Json;
using RentalManager.Domain.Models;

namespace RentalManager.BlazorClient.Services;

public class ProductTypeService
{
    private readonly HttpClient _httpClient;

    public ProductTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ProductType>?> GetProductTypesAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<ProductType>>("api/ProductType");
    }

    public async Task<ProductType?> GetProductTypeAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ProductType>($"api/ProductType/{id}");
    }

    public async Task PostProductTypeAsync(string productType)
    {
        var response = await _httpClient.PostAsJsonAsync("api/ProductType", productType);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task DeleteProductTypeAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/ProductType/{id}");
        response.EnsureSuccessStatusCode();
    }
}