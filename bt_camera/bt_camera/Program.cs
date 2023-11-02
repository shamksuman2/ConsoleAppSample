using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Devices.Onvif;
using Devices.Onvif.Behaviour;
using Devices.Onvif.EventReader;
using Devices.Onvif.Models;
using VisioForge.Core.ONVIF;

namespace Devices.Onvif
{
    class Program
    {
        private const string DefaultDeviceServicePath = "/onvif/device_service";
        private  string _deviceServicePath;
        private IOnvifClientFactory _onvifClientFactory = new OnvifClientFactory();
        //private readonly IDeviceEventReceiverFactory _deviceEventReceiverFactory;
        // private readonly IOnvifClientFactory onvifClientFactory;
        private IConnectionParameters _connectionParameters;

        static async Task Main(string[] args)
        {
            Program p = new Program();
            await p.EventReceiver();
        }

        private async Task EventReceiver()
        {
            //_connectionParameters = new ConnectionParameters("192.168.88.153", 80, "service", "Bosch$123");

            Uri deviceUri;
            string DeviceAddress = "http://192.168.88.153";
            string Login = "service";
            string Password = "Bosch$123";
            var credential = new NetworkCredential(Login, Password);
            var timeout = TimeSpan.FromSeconds(15);


            if (!Uri.TryCreate(DeviceAddress, UriKind.Absolute, out deviceUri))
            {
                Console.WriteLine("Error: Bad device address");
                return;
            }
            _connectionParameters = new ConnectionParameters(deviceUri, credential, timeout);


            var _deviceCapabilities = await GetDeviceCapabilitiesAsync();
            var eventServiceUri = new Uri("http://192.168.88.153/onvif/analytics_service");
            EndpointAddress endpointAddress = await GetSubscriptionEndPointAddress(eventServiceUri);

            var eventService = _onvifClientFactory.CreateClient<EventPortType>(endpointAddress, _connectionParameters, MessageVersion.Soap12WSAddressing10);

            var subscriptionRequest = new CreatePullPointSubscriptionRequest(null, GetTerminationTime(), null, null);

            CreatePullPointSubscriptionResponse response = await eventService.CreatePullPointSubscriptionAsync(subscriptionRequest);

            var subscriptionAddress = response.SubscriptionReference.Address.Value;

            var pullPointSubscriptionClient = _onvifClientFactory.CreateClient<PullPointSubscription>(new Uri(subscriptionAddress), _connectionParameters, MessageVersion.Soap12WSAddressing10);

            var pullRequest = new PullMessagesRequest("PT1S", 1024, null);

            while (true)
            {
                PullMessagesResponse pullMessagesResponse = await pullPointSubscriptionClient.PullMessagesAsync(pullRequest);

                foreach (var messageHolder in pullMessagesResponse.NotificationMessage)
                {
                    var eventXml = messageHolder.Message.InnerXml;

                    var eventDocument = new XmlDocument();
                    eventDocument.LoadXml(eventXml);

                    var analyticsMetadata = eventDocument.GetElementsByTagName("analyticsMetadata");
                    Console.WriteLine(analyticsMetadata.Count + analyticsMetadata.ToString());
                    // Process the analytical metadata here.
                }
            }
        }

        private async Task<Capabilities> GetDeviceCapabilitiesAsync()
        {
            Media deviceClient = CreateDeviceClient();

            return await deviceClient.GetServiceCapabilitiesAsync();

        }
        private Media CreateDeviceClient()
        {
            _deviceServicePath = _connectionParameters.ConnectionUri.AbsolutePath;

            if (_deviceServicePath == "/")
                _deviceServicePath = DefaultDeviceServicePath;

            Uri deviceServiceUri = GetServiceUri(_deviceServicePath);

            var deviceClient = _onvifClientFactory.CreateClient<Media>(deviceServiceUri,
                _connectionParameters, MessageVersion.Soap11);

            return deviceClient;
        }

        private Uri GetServiceUri(string serviceRelativePath)
        {
            return new Uri(_connectionParameters.ConnectionUri, serviceRelativePath);
        }

        private async Task<EndpointAddress> GetSubscriptionEndPointAddress(Uri eventServiceUri)
        {
            var portTypeClient = _onvifClientFactory.CreateClient<EventPortType>(eventServiceUri,
                _connectionParameters, MessageVersion.Soap12WSAddressing10);

            string terminationTime = GetTerminationTime();
            var subscriptionRequest = new CreatePullPointSubscriptionRequest(null, terminationTime, null, null);
            CreatePullPointSubscriptionResponse response =
                await portTypeClient.CreatePullPointSubscriptionAsync(subscriptionRequest);

            var subscriptionRefUri = new Uri(response.SubscriptionReference.Address.Value);

            var adressHeaders = new List<AddressHeader>();

            if (response.SubscriptionReference.ReferenceParameters?.Any != null)
                foreach (System.Xml.XmlElement element in response.SubscriptionReference.ReferenceParameters.Any)
                    adressHeaders.Add(new CustomAddressHeader(element));

            var seviceUri = GetServiceUri(subscriptionRefUri.PathAndQuery);
            var endPointAddress = new EndpointAddress(seviceUri, adressHeaders.ToArray());
            return endPointAddress;
        }
        private static string GetTerminationTime()
        {
            return $"PT{(int)TimeSpan.FromSeconds(30).TotalSeconds}S";
        }
    }
}