using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebConfig
{
    public class SubstitutionConfigurationProvider : ConfigurationProvider
    {
        private readonly Regex _pattern = new Regex(@"(?:{(.*?)})");
        private readonly IConfiguration _configuration;

        public SubstitutionConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void Load()
        {
            Data.Clear();

            foreach (var config in _configuration.AsEnumerable())
            {
                var newValue = config.Value;

                if (!string.IsNullOrWhiteSpace(newValue))
                {
                    newValue = _pattern.Replace(newValue, m =>
                    {
                        var replacement = _configuration[m.Groups[1].Value] ?? m.Value;
                        return replacement;
                    });
                }
                
                Data.Add(config.Key, newValue);
            }
        }
    }
}
