using System;
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
        private readonly IConfiguration _configuration;

        public UrlItemController(UrlShrtDbContext context, IMapper mapper, ILogger<UrlItemController> logger, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<UrlItemViewDto>> CreateUrlItem(UrlItemCreateDto createDto)
        {
            // Generate an alphanumerical slug
            if (createDto.Slug == null)
            {
                string slug;
                int length = ConfigurationHelper.SlugLenght(_configuration, _logger);

                do
                {
                    slug = RandomHelper.NextAlphanumeric(length);
                } while (await _context.UrlItems.AnyAsync(u => u.Slug == slug));
                
                createDto.Slug = slug;
            }
            // Check for duplicates
            else if (await _context.UrlItems.AnyAsync(u => u.Slug == createDto.Slug))
            {
                return Conflict();
            }

            var urlItem = _mapper.Map<UrlItem>(createDto);

            _context.UrlItems.Add(urlItem);
            await _context.SaveChangesAsync();

            return _mapper.Map<UrlItemViewDto>(urlItem);
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult> RedirectFromShortUrl(string slug)
        {
            var urlItem = await _context.UrlItems.SingleOrDefaultAsync(u => u.Slug == slug);
            if (urlItem == null) return NotFound();

            urlItem.Clicks++;
            await _context.SaveChangesAsync();
                
            return Redirect(urlItem.RedirectUrl);
        }
    }
}
