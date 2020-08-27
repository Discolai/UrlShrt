using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShrt.Dtos;
using UrlShrt.Models;

namespace UrlShrt.Profiles
{
    public class UrlItemProfile : Profile
    {
        public UrlItemProfile()
        {
            CreateMap<UrlItemCreateDto, UrlItem>();
            CreateMap<UrlItemViewDto, UrlItem>();
        }
    }
}
