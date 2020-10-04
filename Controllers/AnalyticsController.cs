using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UrlShrt.Data;
using UrlShrt.Dtos;

namespace UrlShrt.Controllers
{
    [Route(UrlItemController.ShortUrlRoot)]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly UrlShrtDbContext _context;
        private readonly ILogger<AnalyticsController> _logger;
        private readonly IMapper _mapper;

        public AnalyticsController(UrlShrtDbContext context, ILogger<AnalyticsController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{slug}/analytics")]
        public async Task<ActionResult> GetAsync(string slug)
        {
            var url = await _context.UrlItems.SingleOrDefaultAsync(u => u.Slug == slug);
            if (url == null)
            {
                ModelState.AddModelError("slug", $"Could not find ${slug}");
                return CommonResponse.CreateResponse(modelState: ModelState, status: 404);
            }

            return CommonResponse.CreateResponse(data: _mapper.Map<AnalyticsViewDto>(url));
        }
    }
}
