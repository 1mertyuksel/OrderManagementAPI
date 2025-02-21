using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using AutoMapper;
using OrderManagementAPI.Dtos;
using OrderManagementAPI.Repository;
using OrderManagementAPI.Services.Abstract;
using Serilog;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMemoryCache _memoryCache;

    public ProductService(IProductRepository productRepository, IMemoryCache memoryCache, IMapper mapper)
    {
        _productRepository = productRepository;
        _memoryCache = memoryCache;
    }

    public async Task<List<ProductDto>> GetProductsAsync(string? category)
    {
        string cacheKey = string.IsNullOrEmpty(category) ? "product_cache_all" : $"product_cache_{category}";

        Log.Information("Checking cache for key: {CacheKey}", cacheKey);

        if (_memoryCache.TryGetValue(cacheKey, out List<ProductDto>? cachedProducts) && cachedProducts != null)
        {
            Log.Information("Cache hit for key: {CacheKey}. Returning cached products.", cacheKey);
            return cachedProducts;
        }

        Log.Information("Cache miss for key: {CacheKey}. Fetching products from repository.", cacheKey);

        var products = await _productRepository.GetByCategoryAsync(category);

        List<ProductDto> productsDto = products;

        Log.Information("Adding products to cache with key: {CacheKey}.", cacheKey);

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

        _memoryCache.Set(cacheKey, productsDto, cacheEntryOptions);

        Log.Information("Products successfully added to cache.");

        return productsDto;
    }
}
