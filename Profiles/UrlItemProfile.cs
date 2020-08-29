using AutoMapper;
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
            CreateMap<UrlItem, UrlItemViewDto>().ForMember(dest => dest.ShortUrl, source => source.MapFrom<ShortUrlResolver>());
        }
    }


    public class ShortUrlResolver : IValueResolver<UrlItem, UrlItemViewDto, string>
    {
        private IHttpContextAccessor _httpContextAccessor;

        public ShortUrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(UrlItem source, UrlItemViewDto destination, string destMember, ResolutionContext context)
        {
            return UrlHelper.Combine(_httpContextAccessor.HttpContext.Request.GetDisplayUrl(), source.Slug);
        }
    }
}
