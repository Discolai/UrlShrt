using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShrt.Services
{
    public class SlugConfiguration : ISlugConfiguration
    {
        public const string SlugLengthField = "SlugLength";
        public const int SlugLengthDefault = 5;

        public int Length { get; }
        public SlugConfiguration(IConfiguration configuration, ILogger<SlugConfiguration> logger)
        {
            var section = configuration.GetSection(SlugLengthField);
            Length = configuration.GetValue<int>(SlugLengthField);
            if (Length <= 0)
            {
                Length = SlugLengthDefault;
                logger.LogInformation("{slugLengthField} was improperly configured, using default length ({defaultLength}) at {time} ETC", SlugLengthField, SlugLengthDefault, DateTime.UtcNow);
            }
        }
    }
}
