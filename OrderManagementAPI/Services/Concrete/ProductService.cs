﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using AutoMapper;
using OrderManagementAPI.Dtos;
using OrderManagementAPI.Repository;
using OrderManagementAPI.Services.Abstract;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMemoryCache memoryCache, IMapper mapper)
    {
        _productRepository = productRepository;
        _memoryCache = memoryCache;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> GetProductsAsync(string? category)
    {
        // Kategoriye özel cache key oluştur
        string cacheKey = string.IsNullOrEmpty(category) ? "product_cache_all" : $"product_cache_{category}";

        if (_memoryCache.TryGetValue(cacheKey, out List<ProductDto>? cachedProducts) && cachedProducts != null)
        {
            return cachedProducts;
        }

        // Veritabanından çek
        var products = await _productRepository.GetByCategoryAsync(category);

        List<ProductDto> productsDto = products;

        
        // Cache’e ekle (30 dakika boyunca saklanacak)
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

        _memoryCache.Set(cacheKey, productsDto, cacheEntryOptions);

        return productsDto;
    }
}
