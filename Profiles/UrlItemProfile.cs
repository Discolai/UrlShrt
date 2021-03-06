﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShrt.Controllers;
using UrlShrt.Dtos;
using UrlShrt.Helpers;
using UrlShrt.Models;

namespace UrlShrt.Profiles
{
    public class UrlItemProfile : Profile
    {
        public UrlItemProfile()
        {
            CreateMap<UrlItemCreateDto, UrlItem>();
            CreateMap<UrlItem, UrlItemViewDto>().ForMember(dest => dest.ShortUrl, source => source.MapFrom<ShortUrlResolverUrlItem>());
            CreateMap<UrlItem, AnalyticsViewDto>().ForMember(dest => dest.ShortUrl, source => source.MapFrom<ShortUrlResolverAnalytics>());
        }
    }


    public class ShortUrlResolverUrlItem : IValueResolver<UrlItem, UrlItemViewDto, string>
    {
        private IHttpContextAccessor _httpContextAccessor;

        public ShortUrlResolverUrlItem(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(UrlItem source, UrlItemViewDto destination, string destMember, ResolutionContext context)
        {
            var uri = new Uri(_httpContextAccessor.HttpContext.Request.GetDisplayUrl());
            //var host = uri.IsDefaultPort ? uri.Host : $"{uri.Host}:{uri.Port}";
            var host = uri.GetLeftPart(UriPartial.Authority);

            return UrlHelper.Combine(host, UrlItemController.ShortUrlRoot, source.Slug);
        }
    }

    public class ShortUrlResolverAnalytics: IValueResolver<UrlItem, AnalyticsViewDto, string>
    {
        private IHttpContextAccessor _httpContextAccessor;

        public ShortUrlResolverAnalytics(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(UrlItem source, AnalyticsViewDto destination, string destMember, ResolutionContext context)
        {
            var uri = new Uri(_httpContextAccessor.HttpContext.Request.GetDisplayUrl());
            var host = uri.GetLeftPart(UriPartial.Authority);

            return UrlHelper.Combine(host, UrlItemController.ShortUrlRoot, source.Slug);
        }
    }
}
