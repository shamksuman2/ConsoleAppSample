using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            // Set the Solace REST messaging API URL
            string url = "https://mr-connection-l7ua5om65da.messaging.solace.cloud:9443";

            // Set the Solace API credentials
            string username = "solace-cloud-client";
            string password = "pflp78r0u6ej8q7hntd7l4ncqp";

            // Create the message payload
            string messagePayload = "Hello, Solace from Sham!";



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


            string responseMessage = string.IsNullOrEmpty(messagePayload)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {nameof(messagePayload)}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
