﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UrlShrt.Data;
using UrlShrt.Dtos;
using UrlShrt.Helpers;
using UrlShrt.Models;
using UrlShrt.Services;

namespace UrlShrt.Controllers
{
    [Route(UrlItemController.ShortUrlRoot)]
    [ApiController]
    public class UrlItemController : ControllerBase
    {
        public const string ShortUrlRoot = "u/";

        private readonly UrlShrtDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UrlItemController> _logger;
        private readonly ISlugConfiguration _slugConfiguration;

        public UrlItemController(UrlShrtDbContext context, IMapper mapper, ILogger<UrlItemController> logger, ISlugConfiguration slugConfiguration)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _slugConfiguration = slugConfiguration;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUrlItemAsync(UrlItemCreateDto createDto)
        {
            // Generate an alphanumerical slug
            if (string.IsNullOrEmpty(createDto.Slug))
            {
                string slug;
                do
                {
                    slug = RandomHelper.NextAlphanumeric(_slugConfiguration.Length);
                } while (await _context.UrlItems.AnyAsync(u => u.Slug == slug));
                
                createDto.Slug = slug;
            }

            // Check for duplicates
            else if (await _context.UrlItems.AnyAsync(u => u.Slug == createDto.Slug))
            {
                ModelState.AddModelError("Slug", "The slug must be unique");
                return CommonResponse.CreateResponse(modelState: ModelState);
            }

            var urlItem = _mapper.Map<UrlItem>(createDto);

            _context.UrlItems.Add(urlItem);
            await _context.SaveChangesAsync();

            var viewDto = _mapper.Map<UrlItemViewDto>(urlItem);
            _logger.LogInformation("{method}: {shortUrl} -> {redirectUrl}, {time} UTC", nameof(CreateUrlItemAsync), viewDto.ShortUrl, viewDto.RedirectUrl, DateTime.UtcNow);

            return CommonResponse.CreateResponse(data: viewDto);

        }

        [HttpGet("{slug}")]
        public async Task<ActionResult> RedirectFromShortUrlAsync(string slug)
        {
            var urlItem = await _context.UrlItems.SingleOrDefaultAsync(u => u.Slug == slug);
            if (urlItem == null)
            {
                ModelState.AddModelError("slug", $"Could not find ${slug}");
                return CommonResponse.CreateResponse(modelState: ModelState, status: 404);
            }

            urlItem.Clicks++;
            await _context.SaveChangesAsync();

            _logger.LogInformation("{method}: {shortUrl} -> {redirectUrl}, {time} UTC", nameof(RedirectFromShortUrlAsync), Request.GetEncodedUrl(), urlItem.RedirectUrl, DateTime.UtcNow);

            return Redirect(urlItem.RedirectUrl);
        }
    }
}
