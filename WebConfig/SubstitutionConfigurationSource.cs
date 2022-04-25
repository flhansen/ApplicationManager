using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfig
{
    public class SubstitutionConfigurationSource : IConfigurationSource
    {
        private readonly IConfiguration _configuration;

        public SubstitutionConfigurationSource(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SubstitutionConfigurationProvider(_configuration);
        }
    }
}
