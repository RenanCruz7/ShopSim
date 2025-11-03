﻿using ShopSim.DTOs;
using ShopSim.Models;
using AutoMapper;

namespace ShopSim.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();
        CreateMap<Product, ProductReadDto>();
    }
}