﻿using ShopSim.DTOs;
using ShopSim.Models;
using AutoMapper;

namespace ShopSim.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // Product mappings
        CreateMap<Product, ProductReadDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();

        // Category mappings
        CreateMap<Category, CategoryReadDto>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();

        // User mappings
        CreateMap<User, UserReadDto>();
        CreateMap<UserRegisterDto, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower()));

        // Order mappings
        CreateMap<Order, OrderReadDto>();
        CreateMap<OrderItem, OrderItemReadDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
    }
}