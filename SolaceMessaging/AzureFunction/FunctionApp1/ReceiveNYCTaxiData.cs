using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Newtonsoft.Json;
using System.Data.SqlTypes;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ReceiveNYCTaxiDataNS
{
    public static class ReceiveNYCTaxiData
    {
        [FunctionName("ReceiveNYCTaxiDataFunc")]
        public static async Task<IActionResult> ReceiveNYCTaxiDataFunc(
               [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)

        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var jobject = JObject.Parse(requestBody);

            //var flattened = jobject.Flattend();
            var flattenedjsonString = JsonConvert.SerializeObject(jobject.Values(), Formatting.Indented);
            Console.WriteLine(flattenedjsonString);
            log.LogInformation(flattenedjsonString);
            string powerBiurl = $"https://apt.powerbi.com/bete/ce001-a4-74b1-40fc-9184-edb4a21-35d5/datasets/6ffa-301-1668-4-3b-97ff-589-81285566/rss";
            HttpClient client = new HttpClient();
            HttpContent content = new StringContent($"1 {flattenedjsonString} 1");
            HttpResponseMessage response = await client.PostAsync(powerBiurl, content);
            response.EnsureSuccessStatusCode();
            log.LogInformation(response.ToString());
            return new OkObjectResult(response);
        }
    }
}
