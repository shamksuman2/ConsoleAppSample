
using System.Net;
using System.Runtime;
using System.Text;
using System.Xml.Serialization;

namespace OnvifMetadata
{
    class Program
    {
        static async Task Main(string[] args)
        {

            //var media = onvifDev.GetMediaService();
            var profilesList = GetProfile();
            //var metadataConfigurationsList = media.GetMetadataConfigurations();

            //// Get the token for the first configuration
            //// and then configure for our needs
            //var metadataConfiguration = metadataConfigurationsList[0];
            //var metadataToken = metadataConfiguration.Token; // Attribute

            //// See if our desired metadata configuration already exists
            //var configuration = media.GetMetadataConfiguration(ConfigurationToken: metadataToken);
            //configuration.Name = "metadata";

            //// Must setup an Events Filter to get any events
            //// Set Dialect attribute
            //configuration.Events.Filter.TopicExpression.Dialect = "http://www.onvif.org/ver10/tev/topicExpression/ConcreteSet";

            //// Set TopicExpression to get everything under Device topic
            //configuration.Events.Filter.TopicExpression = "tns1:Device//.";

            //// An alternative way to get all events would be to use:
            //// configuration.Events.Filter.TopicExpression = null;
            //media.SetMetadataConfiguration(Configuration: configuration);

            //// See if our desired profile already exists
            //var profile = await media.GetProfile(ProfileToken: "metadata");
            //if (profile == null)
            //{
            //    // No, our desired Profile does not exist,
            //    // use CreateProfile to create it:
            //    // CreateProfile with Name and Token:
            //    profile = media.CreateProfile(Name: "metadata apg", Token: "metadata");
            //    // We get an empty Profile back
            //}

            //// Check if we have a MetadataConfiguration in the Profile:
            //// Not a strict requirement since we could choose to always add it,
            //// but let's save that call if we can.
            //if (profile.MetadataConfiguration == null)
            //{
            //    // No MetadataConfiguration, let's add it:
            //    // AddMetadataConfiguration with our Profile(Token) and
            //    // Metadata ConfigurationToken
            //    media.AddMetadataConfiguration(ProfileToken: "metadata", ConfigurationToken: metadataToken);
            //}

            //// Setup stream configuration (RTP/RTSP) and the desired Profile:
            //var streamSetup = new StreamSetup { Stream = "RTP-Unicast", Transport = new Transport { Protocol = "RTSP", Tunnel = null } };

            //// Get stream URI
            //var mediaUri = media.GetStreamUri(StreamSetup: streamSetup, ProfileToken: "metadata");

            //// Fetch the RTSP stream and handle the metadata
            //FetchMetadataRtspStream(mediaUri.Uri);
        }



        // Method to test ONVIF metadata streaming
        //async void TestMetadataStreaming(OnvifDevice onvifDev)
        //{
        //    
        //}

        //void FetchMetadataRtspStream(string streamUri)
        //{
        //    // Implement logic to fetch and handle the RTSP stream with metadata
        //    // ...
        //}

        private static async Task<List<Profile>> GetProfile() {

            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                IWebProxy proxy = WebRequest.GetSystemWebProxy();
                proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
                handler.UseDefaultCredentials = true;
                handler.Proxy = proxy;

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

            return new List<Profile>();
        }

        private static async Task<List<MetadataConfiguration>> GetMetadataConfigurations()
        {

            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                IWebProxy proxy = WebRequest.GetSystemWebProxy();
                proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
                handler.UseDefaultCredentials = true;
                handler.Proxy = proxy;

                using (HttpClient client = new HttpClient(handler))
                {
                    var byteArray = Encoding.ASCII.GetBytes($"service:Bosch$123");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    var messagePayload = "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:trt=\"http://www.onvif.org/ver10/media/wsdl\">\r\n    <soap:Header/>\r\n    <soap:Body>\r\n        <trt:GetMetadataConfigurations/>\r\n    </soap:Body>\r\n</soap:Envelope>\r\n";
                    // Create the HTTP request body with the message payload
                    var content = new StringContent(messagePayload, Encoding.UTF8, "text/plain");

                    // Send the POST request to publish the message
                    HttpResponseMessage response = await client.PostAsync("http://192.168.88.153/onvif/media_service", content);

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return new List<MetadataConfiguration>();
        }

    }

    internal class MetadataConfiguration
    {
    }

    public class Profile
    {

        public string Name { get; set; }

        public string VideoSourceConfiguration { get; set; }

        public string AudioSourceConfiguration { get; set; }

        public string VideoEncoderConfiguration  { get; set; }

        public string MetadataConfiguration { get; set; }


    }
}


// See https://aka.ms/new-console-template for more information

