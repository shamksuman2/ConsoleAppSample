using System;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        //string url = "https://mr-connection-8cbir63661i.messaging.solace.cloud:9443";
        //string username = "solace-cloud-client";
        //string password = "21atvu2kvfqt83okvnqhp2r53d";

        string username = "solace-cloud-client";
        string password = "pflp78r0u6ej8q7hntd7l4ncqp";
        string url = "https://mr-connection-l7ua5om65da.messaging.solace.cloud:9443";
        string messagePayload = "Hello, Solace from Visual Studio!";


        // Create a HttpClientHandler with proxy settings and credentials
        //var handler = new HttpClientHandler
        //{
        //    Proxy = new WebProxy("http://rb-proxy-de.bosch.com:8080/"),
        //    UseProxy = true,
        //    Credentials = new NetworkCredential(username, password)
        //};
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

        HttpClientHandler handler = new HttpClientHandler();
        IWebProxy proxy = WebRequest.GetSystemWebProxy();
        proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
        handler.UseDefaultCredentials = true;
        handler.Proxy = proxy;

        using var httpClient = new HttpClient(handler);

        // Create the HttpClient using the HttpClientHandler
        using (HttpClient client = new HttpClient(handler))
        {

            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

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
    }

    private static void WriteToFile(string path, string data)
    {
        using (var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
        using (var sw = new StreamWriter(fs))
        {
            sw.WriteLine(data + Environment.NewLine + "-------------------------------------------------------------" + Environment.NewLine);
        }
    }
}

