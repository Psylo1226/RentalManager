using System.Net.Http.Json;
using RentalManager.BlazorClient.Pages.Products;
using RentalManager.Domain.Models;

namespace RentalManager.BlazorClient.Services;

public class ProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Product>?> GetProductsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Product>>("api/Product");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Product>($"api/Product/{id}");
    }

    public async Task PostProductAsync(ProductPage.ProductDto product)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Product", product);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteProductAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Product/{id}");
        response.EnsureSuccessStatusCode();
    }
}