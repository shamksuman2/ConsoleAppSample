using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using SolaceSystems.Solclient.Messaging;
using System.Threading;
using ISession = SolaceSystems.Solclient.Messaging.ISession;
using System.Text;
using System.IO;
using System.Net.Http;

namespace ReceiveNYCTaxiDataNS
{

    public static class http_publisher_NYCTaxiData
    {

        static string VPNName { get; set; }
        static string UserName { get; set; }
        static string Password { get; set; }
        const int DefaultReconnectRetries = 3;
        private static ISession Session = null;
        private static EventWaitHandle WaitEventWaitHandle = new AutoResetEvent(false);
        static ILogger  log = null;
        [FunctionName("PublisherNYCTaxiDataFunc")]
        public static async Task<IActionResult> http_publisher_NYCTaxiData_fn(
               [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log1)

        {
            // Set the Solace REST messaging API URL
            string url = "https://mr-connection-l7ua5om65da.messaging.solace.cloud:9443";

            string username = "solace-cloud-client";
            string password = "pflp78r0u6ej8q7hntd7l4ncqp";
            string messagePayload = "Hello, Solace from Sham!";
            // Create a HttpClientHandler with proxy settings and credentials
            //var handler = new HttpClientHandler
            //{
            //    Proxy = new WebProxy("http://rb-proxy-de.bosch.com:8080"),
            //    UseProxy = true,
            //    Credentials = new NetworkCredential(username, password)
            //};

            // Create the HttpClient using the HttpClientHandler
            using (HttpClient client = new HttpClient())
            {
                // Create the HTTP request body with the message payload
                var content = new StringContent(messagePayload, Encoding.UTF8, "text/plain");

                // Send the POST request to publish the message
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Check the response status code
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Message published successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to publish message. Status code: {response.StatusCode}");
                }
            }
            return new OkObjectResult(null);
        }

    }


}
