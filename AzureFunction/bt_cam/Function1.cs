using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace bt_cam
{
    public static class Function1
    {
        [FunctionName("camera1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string name = req.Query["name"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //object data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? (!string.IsNullOrEmpty(requestBody) ? requestBody.ToString() + "1 " : requestBody + "2");
            log.LogInformation($"Cam1 message {name}");
            return new OkObjectResult(name);
        }
    }
}
