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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UrlShrt.Data;
using UrlShrt.Dtos;
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

        public UrlItemController(UrlShrtDbContext context, IMapper mapper, ILogger<UrlItemController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<UrlItemViewDto>> CreateUrlItem(UrlItemCreateDto createDto)
        {
            var urlItem = _mapper.Map<UrlItem>(createDto);

            _context.UrlItems.Add(urlItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Catched exception in {method} at {time}", nameof(CreateUrlItem), DateTime.UtcNow);
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            return _mapper.Map<UrlItemViewDto>(urlItem);
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult> RedirectFromShortUrl(string slug)
        {
            try
            {
                var urlItem = await _context.UrlItems.SingleAsync(u => u.Slug == slug);
                urlItem.Clicks++;
                await _context.SaveChangesAsync();
                
                return Redirect(urlItem.RedirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Catched exception in {method} at {time}", nameof(CreateUrlItem), DateTime.UtcNow);
                return NotFound();
            }
        }
    }
}
