using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfig
{
    public static class SecretConfigurationExtensions
    {
        /// <summary>
        /// The secret file path can be set using environment variables.
        /// Otherwise the default location will be in the home directory under
        /// <code>/path/to/home/.secret/secrets.json</code>.
        /// </summary>
        public static readonly string SecretFilePath =
            Environment.GetEnvironmentVariable("APPLICATION_MANAGER_SECRET_FILE",
                EnvironmentVariableTarget.User) ?? $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.secret/secrets.json";

        /// <summary>
        /// Adds a secret file to the configuration system and creates an empty
        /// one, if none exist. The default path is `$HOME/.secret.secrets.json`.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="secretFilePath">The path to the secrets file</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddSecretFile(this IConfigurationBuilder builder, string secretFilePath)
        {
            // Create the secret folder, if it doesn't exist.
            var secretDirectory = Path.GetDirectoryName(secretFilePath);
            if (!Directory.Exists(secretDirectory) && !string.IsNullOrWhiteSpace(secretDirectory))
                Directory.CreateDirectory(secretDirectory);

            // Check if the secret file exists. If not, create an empty one.
            if (!File.Exists(secretFilePath))
                using (var stream = File.CreateText(secretFilePath))
                    stream.Write("{}");

            // In any case, add the secret file to the configuration builder.
            builder.AddJsonFile(secretFilePath, optional: false, reloadOnChange: false);
            return builder;
        }

        /// <summary>
        /// Adds a secret file to the configuration system and creates an empty
        /// one, if none exist. The default path is <code>/path/to/home/.secret/secrets.json</code>.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddSecretFile(this IConfigurationBuilder builder)
        {
            AddSecretFile(builder, SecretFilePath);
            return builder;
        }
    }
}
