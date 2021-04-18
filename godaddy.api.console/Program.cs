using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace godaddy.api.console
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        private static string dnsRecordTypes = "A, AAAA, CNAME, MX, NS, SOA, SRV, TXT";

        private static string authorizationKey = "Authorization";
        private static string authorizationValue = "";

        private static string baseGodaddyDomainsUrl = "https://api.godaddy.com/v1/domains/";

        private static string novalenniumUri = "novalennium.com/records/CNAME";

        private static string dnsRecordName = "_acme-challenge.blah.com";
        private static string method = "GET";
        private static string domain = "yourdomainname.com";
        private static string challenge = "blahblahblah";
        private static string auth = "sso-key blehblehbleh";


        public static void WriteArgs(IConfiguration config)
        {
            Console.WriteLine($"method: '{config["method"]}'");
            Console.WriteLine($"auth: '{config["auth"]}'");
            Console.WriteLine($"domain: '{config["domain"]}'");
            Console.WriteLine($"challenge: '{config["Challenge"]}'");
        }

        static async Task Main(string[] args)
        {

        }

        private static async Task printEntries()
        {
            List<DnsEntry> entries = await ProcessDnsEntries();

            foreach (DnsEntry entry in entries)
            {
                Console.WriteLine(String.Format("data: {0}, name: {1}, TTL: {2}", entry.Data, entry.Name, entry.TTL));

            }
        }
        private static async Task<List<DnsEntry>> ProcessDnsEntries()
        {
            client.DefaultRequestHeaders.Add(authorizationKey, authorizationValue);
            string requestUri = baseGodaddyDomainsUrl + novalenniumUri;

            var streamTask = client.GetStreamAsync(requestUri);

            var entries = await JsonSerializer.DeserializeAsync<List<DnsEntry>>(await streamTask);

            return entries;

        }
        private static void PrintHelp()
        {
            Console.WriteLine("Simple .net core wrapper for the Godaddy REST api.");
            Console.WriteLine("Geared towards being used for LetsEncrypt wildcard certificate automation");

            Console.WriteLine("--domain = GET to list records of a certain type for a domain (OR) POST to set a value for an existing/new record");
            Console.WriteLine("--auth = Godaddy API key/token. Usually of the form: 'sso-key <string of characters numbers symbols>'. Since there's a space in it, you must enclose it in quotes\" ");
            Console.WriteLine("--challenge = Used only with POST method; This is the value letsencrypt wants your DNS record to contain as a response to the challenge");
        }
    }
}
