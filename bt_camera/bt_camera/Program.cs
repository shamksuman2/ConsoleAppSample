using Nager.VideoStream;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using FFmpeg.AutoGen;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;



namespace cam1_receiver.Controllers
{

  

    
    class Program
    {
        public static async Task Main()
        {

            try
            {
                //HttpClientHandler handler = new HttpClientHandler();
                //IWebProxy proxy = WebRequest.GetSystemWebProxy();
                //proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
                //handler.UseDefaultCredentials = true;
                //handler.Proxy = proxy;

                using (HttpClient client = new HttpClient())
                {
                    var byteArray = Encoding.ASCII.GetBytes($"service:Bosch$123");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    var messagePayload = "&lt;soap:Envelope xmlns:soap=&quot;http://www.w3.org/2003/05/soap-envelope&quot; xmlns:trt=&quot;http://www.onvif.org/ver10/media/wsdl&quot;&gt;\r\n    &lt;soap:Header/&gt;\r\n    &lt;soap:Body&gt;\r\n        &lt;trt:GetProfiles/&gt;\r\n    &lt;/soap:Body&gt;\r\n&lt;/soap:Envelope&gt;";
                    // Create the HTTP request body with the message payload
                    var content = new StringContent("", Encoding.UTF8, "text/plain");
                    try
                    {
                        HttpResponseMessage response = await client.PostAsync("http://192.168.88.153/onvif/media_service", content);

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Message published successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to publish message. Status code: {response.StatusCode}");
                        }

                    }
                    catch (Exception e)
                    {


                    }
                    // Send the POST request to publish the message

                    // Check the response status code
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }

    }


    /*
    public class Program
    {
        //static void Main()
        //{
        //    // Replace with your camera's IP address and port
        //    string cameraUrl = "http://192.168.88.153:80"; // Example URL
        //    //"rtsp://service:Bosch_123@192.168.88.153/rtsp_tunnel"
        //    // Replace with the actual endpoint for retrieving video metadata
        //    string metadataEndpoint = "/get_video_metadata"; // Example endpoint

        //    try
        //    {

        //        // Create a WebClient for making HTTP requests
        //        using (WebClient client = new WebClient())
        //        {
        //            // Combine the camera URL and metadata endpoint
        //            string fullUrl = cameraUrl + metadataEndpoint;

        //            // Download the XML data
        //            string xmlData = client.DownloadString(fullUrl);

        //            // Save the XML data to a file or process it as needed
        //            File.WriteAllText("video_metadata.xml", xmlData);

        //            Console.WriteLine("Video metadata saved to video_metadata.xml");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error: " + ex.Message);
        //    }
        //}

        async Task Main1()
        {
            // Replace with the actual camera IP address and port
            string cameraIpAddress = "192.168.88.153";
            int cameraPort = 80; // Default HTTP port

            // Replace with the camera's API endpoint for retrieving metadata
            string metadataEndpoint = "/api/get_metadata";

            // Replace with your camera's authentication credentials (if required)
            string username = "service";
            string password = "Bosch_123";

            // Replace with your proxy server information and credentials
            string proxyIpAddress = "http://rb-proxy-de.bosch.com:8080/";
            int proxyPort = 8080; // Replace with the actual proxy port
            string proxyUsername = "zzu2kor";
            string proxyPassword = "******";

            using (var httpClientHandler = new HttpClientHandler())
            {
                // Set up proxy authentication if your proxy server requires it
                if (!string.IsNullOrEmpty(proxyUsername) && !string.IsNullOrEmpty(proxyPassword))
                {
                    httpClientHandler.Proxy = new WebProxy(proxyIpAddress, proxyPort);
                    httpClientHandler.Proxy.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
                }

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    // Set up basic authentication if your camera requires it
                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    {
                        var byteArray = System.Text.Encoding.ASCII.GetBytes($"{username}:{password}");
                        httpClient.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }

                    // Construct the full URL for the metadata endpoint
                    string apiUrl = $"http://{cameraIpAddress}:{cameraPort}{metadataEndpoint}";

                    try
                    {
                        // Send an HTTP GET request to the camera to retrieve metadata
                        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            // Read the metadata response
                            string metadataJson = await response.Content.ReadAsStringAsync();
                            Console.WriteLine("Video Metadata:");
                            Console.WriteLine(metadataJson);
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }

        static async Task Main(string[] args)
        {
            string cameraEndpoint = "http://192.168.88.153/onvif/media_service";

            // Replace with your camera's username and password
            string username = "service";
            string password = "Bosch_123";

            OnvifClient onvifClient = new OnvifClient(cameraEndpoint, username, password);

            try
            {
                // Send the GetProfiles request and get the response
                string response = await onvifClient.GetProfilesAsync();

                // Print the response (XML format)
                Console.WriteLine("Response:");
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }


    public class OnvifClient
    {
        private readonly string _cameraEndpoint;
        private readonly string _username;
        private readonly string _password;

        public OnvifClient(string cameraEndpoint, string username, string password)
        {
            _cameraEndpoint = cameraEndpoint;
            _username = username;
            _password = password;

        }

        public async Task<string> GetProfilesAsync()
        {
            try
            {
                // Replace with your proxy server information and credentials
                string proxyIpAddress = "http://rb-proxy-de.bosch.com:8080/";
                int proxyPort = 8080; // Replace with the actual proxy port
                string proxyUsername = "zzu2kor";
                string proxyPassword = "sham82!HOSCH";

                //Create a HttpClientHandler with proxy settings and credentials

                using (var httpClientHandler = new HttpClientHandler())
                {
                    //Set up proxy authentication if your proxy server requires it
                if (!string.IsNullOrEmpty(proxyUsername) && !string.IsNullOrEmpty(proxyPassword))
                    {
                        httpClientHandler.Proxy = new WebProxy(proxyIpAddress, proxyPort);
                        httpClientHandler.Proxy.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
                    }
                    using (var httpClient = new HttpClient(httpClientHandler))
                    {

                        //_httpClient = new HttpClient();
                        //_httpClient.DefaultRequestHeaders.Authorization =
                        //    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        //        "Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}")));

                        // Set up basic authentication if your camera requires it
                        if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                        {
                            var byteArray = System.Text.Encoding.ASCII.GetBytes($"{_username}:{_password}");
                            httpClient.DefaultRequestHeaders.Authorization =
                                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                        }

                        // Construct the full URL for the metadata endpoint
                        //string apiUrl = $"http://{cameraIpAddress}:{cameraPort}{metadataEndpoint}";

                        try
                        {
                            // Send an HTTP GET request to the camera to retrieve metadata
                            string soapRequest = @"
                                <s:Envelope xmlns:s='http://www.w3.org/2003/05/soap-envelope' xmlns:trt='http://www.onvif.org/ver10/media/wsdl'>
                                    <s:Body>
                                        <trt:GetProfiles/>
                                    </s:Body>
                                </s:Envelope>";

                            // Set the content type and SOAPAction header
                            var requestContent = new StringContent(soapRequest, System.Text.Encoding.UTF8, "application/soap+xml");
                            requestContent.Headers.Add("SOAPAction", "http://www.onvif.org/ver10/media/wsdl/GetProfiles");

                            // Send the SOAP request
                            HttpResponseMessage response = await httpClient.PostAsync(_cameraEndpoint, requestContent);

                            // Check if the request was successful
                            response.EnsureSuccessStatusCode();

                            // Read the response content as a string
                            string responseContent = await response.Content.ReadAsStringAsync();
                            return responseContent;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                } 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }


    } */

}


/*
 public static void Main(string[] args)
 {
     // Replace with your camera's RTSP URL
     string rtspUrl = "rtsp://service:Bosch_123@192.168.88.153/rtsp_tunnel?p=0&h26x=4&vcd=2";

     // Set up FFmpeg process
     Process ffmpegProcess = new Process();
     ffmpegProcess.StartInfo.FileName = "ffmpeg"; // Ensure FFmpeg is in your system's PATH
     ffmpegProcess.StartInfo.Arguments = $"-i {rtspUrl} -f null -"; // Capture video to null output
     ffmpegProcess.StartInfo.RedirectStandardError = true;
     ffmpegProcess.StartInfo.UseShellExecute = false;
     ffmpegProcess.StartInfo.CreateNoWindow = true;

     // Handle FFmpeg error data (which contains metadata)
     ffmpegProcess.ErrorDataReceived += (sender, e) =>
     {
         if (!string.IsNullOrEmpty(e.Data))
         {
             // Parse and process metadata here
             string metadata = e.Data;
             Console.WriteLine(metadata);
         }
     };

     // Start FFmpeg process
     ffmpegProcess.Start();
     ffmpegProcess.BeginErrorReadLine();

     // Wait for user input to exit
     Console.WriteLine("Press any key to exit...");
     Console.ReadKey();

     // Stop and clean up FFmpeg process
     ffmpegProcess.CancelErrorRead();
     ffmpegProcess.WaitForExit();
     ffmpegProcess.Close();
 }
*/
