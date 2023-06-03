﻿using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;

namespace MDP.Network.Http
{
    [MDP.Registration.Factory<IServiceCollection, HttpClientFactorySetting>("MDP.Network.Http", "HttpClientFactory")]
    public class HttpClientFactoryFactory
    {
        // Methods
        public void ConfigureService(IServiceCollection serviceCollection, HttpClientFactorySetting httpClientFactorySetting)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (httpClientFactorySetting == null) throw new ArgumentException($"{nameof(httpClientFactorySetting)}=null");

            #endregion

            // EndpointList
            var endpointList = httpClientFactorySetting.Endpoints ?? new Dictionary<string, Endpoint>();
            if (endpointList.Count <= 0)
            {
                // HttpClient
                serviceCollection.AddHttpClient();

                // Return
                return;
            }

            // HttpClientFactory
            foreach (var endpoint in endpointList)
            {
                // Name
                var name = endpoint.Key;
                if (string.IsNullOrEmpty(name) == true) throw new InvalidOperationException($"{nameof(name)}=null");
                     
                // HttpClient
                serviceCollection.AddHttpClient(name, httpClient =>
                {
                    // BaseAddress
                    var baseAddress = endpoint.Value?.BaseAddress;
                    if (string.IsNullOrEmpty(baseAddress) == false)
                    {
                        // EndsWith
                        if (baseAddress.EndsWith(@"/") == false) baseAddress += @"/";

                        // Set
                        httpClient.BaseAddress = new Uri(baseAddress);
                    }

                    // Headers
                    var headers = endpoint.Value?.Headers ?? new Dictionary<string, string>();
                    foreach (var header in headers)
                    {
                        // Add
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                });
            }
        }


        // Class
        public class HttpClientFactorySetting
        {
            // Properties
            public Dictionary<string, Endpoint> Endpoints { get; set; } = new Dictionary<string, Endpoint>();
        }

        public class Endpoint
        {
            // Properties
            public string BaseAddress { get; set; } = string.Empty;

            public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        }
    }
}
