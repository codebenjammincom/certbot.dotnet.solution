using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace godaddy.api
{
    public class PutRequest
    {
        string _authorizationkey = "";
        string _authroizationValue = "";
        string _domain = "";
        string _recordType = "";
        string _recordName = "";
        string _recordValue = "";

        private string baseGodaddyDomainsUrl = "https://api.godaddy.com/v1/domains/";

        private string stringToFormat = "{0}/records/{1}";

        public PutRequest(string authorizationKey, string authorizationValue, string domain, string recordType, string recordName, string recordValue)
        {
            _authorizationkey = authorizationKey;
            _authroizationValue = authorizationValue;
            _domain = domain;
            _recordType = recordType;
            _recordName = recordName;
            _recordValue = recordValue;
        }

        public async Task<bool> MakeRequest(bool dryRun = false)
        {
            string requestUri = baseGodaddyDomainsUrl + $"{_domain}/records/{_recordType}/{_recordName}";
            string content = "[{\"data\":\"" + _recordValue + "\"}]";
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            string uri = baseGodaddyDomainsUrl + stringToFormat;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(_authorizationkey, _authroizationValue);
                
                Console.WriteLine($"Making request to: {requestUri}");

                using (var response = await client.PutAsync(requestUri, httpContent))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                        //do your error logging and/or retry logic
                    }
                }
            }

            
        }

    }
}
