using godaddy.api;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace certbot.console
{
    class Program
    {
        /// <summary>
        /// https://certbot.eff.org/docs/using.html#pre-and-post-validation-hooks
        /// These environment variables are set by certbot (* - used by manual-auth-hook)
        /// *CERTBOT_DOMAIN: The domain being authenticated
        /// *CERTBOT_VALIDATION: The validation string
        /// CERTBOT_TOKEN: Resource name part of the HTTP-01 challenge(HTTP-01 only)
        /// CERTBOT_REMAINING_CHALLENGES: Number of challenges remaining after the current challenge
        /// CERTBOT_ALL_DOMAINS: A comma-separated list of all domains challenged for the current certificate
        /// </summary>

        private static IConfiguration _configuration = null;

        public static void WriteArgs(IConfiguration config)
        {
            Console.WriteLine($"authorizationKey: '{config["authorizationKey"]}'");
            Console.WriteLine($"authorizationValue: '{config["authorizationValue"]}'");
            Console.WriteLine($"ApiBase: '{config["ApiBase"]}'");
            Console.WriteLine($"challengeRecordName: '{config["challengeRecordName"]}'");
            Console.WriteLine($"CERTBOT_DOMAIN: '{config["CERTBOT_DOMAIN"]}'");
            Console.WriteLine($"CERTBOT_VALIDATION: '{config["CERTBOT_VALIDATION"]}'");
            Console.WriteLine($"certbot_dryRun: '{config["certbot_dryRun"]}'");
            Console.WriteLine($"timetosleep: '{config["timetosleep"]}'");
        }

        static async Task Main(string[] args)
        {
            var switchMappings = new Dictionary<string, string>()
            {
                { "-m", "method" },
                { "-a", "auth" },
                { "-d", "domain" },
                { "-c", "challenge" },
            };
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddCommandLine(args, switchMappings)
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            WriteArgs(_configuration);

            var pr = new PutRequest(_configuration["authorizationKey"],
                _configuration["authorizationValue"],
                _configuration["CERTBOT_DOMAIN"],
                "TXT",
                _configuration["challengeRecordName"],
                _configuration["CERTBOT_VALIDATION"]
                );
            bool dryRun = true;
            bool.TryParse(_configuration["certbot_dryRun"], out dryRun);

            bool result = await pr.MakeRequest(dryRun);
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"Sleeping for: {_configuration["timetosleep"]} milliseconds");
            if (result)
                Thread.Sleep(int.Parse(_configuration["timetosleep"]));
        }
    }
}
