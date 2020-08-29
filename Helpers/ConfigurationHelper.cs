using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShrt.Helpers
{
    public class ConfigurationHelper
    {
        private const int DefaultSlugLength = 5;
        private const string SlugLengthFieldName = "SlugLength";

        public static int SlugLenght(IConfiguration configuration, ILogger logger)
        {
            int length;
            var section = configuration.GetSection(SlugLengthFieldName);

            if (!section.Exists())
            {
                logger.LogWarning("{fieldName} was not specified in configuration, using default of {slugLength}",SlugLengthFieldName, DefaultSlugLength);
                length = DefaultSlugLength;
            }
            else { length = int.Parse(section.Value); }

            return length;
        }
    }
}
