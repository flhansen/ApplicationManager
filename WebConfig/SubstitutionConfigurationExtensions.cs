using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfig
{
    public static class SubstitutionConfigurationExtensions
    {
        public static IConfigurationBuilder AddSubstitution(this IConfigurationBuilder builder, IConfiguration configuration)
        {
            builder.Add(new SubstitutionConfigurationSource(configuration));
            return builder;
        }
    }
}
