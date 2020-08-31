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
        public async Task<ActionResult<UrlItemViewDto>> CreateUrlItem(UrlItemCreateDto createDto)
        {
            // Generate an alphanumerical slug
            if (createDto.Slug == null)
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
                return ValidationProblem(modelStateDictionary: ModelState, statusCode: 409);
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
