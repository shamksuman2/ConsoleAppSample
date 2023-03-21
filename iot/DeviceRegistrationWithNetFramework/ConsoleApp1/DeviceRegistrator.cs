
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Exceptions;

namespace DeviceRegWithNetFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DeviceRegWithNetFramework.Library;
    //using Dover.Devices.IoTHub;
    using Flurl.Http;
    //using Microsoft.Azure.Devices.Client;
    //using Microsoft.Azure.Devices.Client.Exceptions;
    using Polly;
    using Serilog;

    public class DeviceRegistrator : IRegistrator
    {
        private readonly IDeviceClient _deviceClient;
        private readonly Func<string, IFlurlRequest> _registerDeviceRequest;
        private readonly ILogger _log;
        private readonly string _deviceId;
        private readonly string _macAddress;
        private readonly IEnumerable<IDeviceBoundMethodHandler> _methodHandlers;
        private readonly IEnumerable<TimeSpan> _retryIntervals;

        public DeviceRegistrator(ILogger log, IDeviceClient deviceClient, Func<string, IFlurlRequest> registerDeviceRequest,
            string deviceId, IEnumerable<IDeviceBoundMethodHandler> methodHandlers = null,
            IEnumerable<TimeSpan> retryIntervals = null)
        {
            _deviceClient = deviceClient;
            _registerDeviceRequest = registerDeviceRequest;
            _log = log.ForContext<DeviceRegistrator>();
            _deviceId = deviceId;
            _macAddress = MacAddressAccessor.GetMacAddress();
            _methodHandlers = methodHandlers;
            _retryIntervals = retryIntervals ?? Enumerable.Range(0, 10).Select(i => TimeSpan.FromSeconds(i * 3));
        }

        public async Task RegisterDevice()
        {
            try
            {
                var msg = $"initializing device client with device Id: {_deviceId}";
                Console.WriteLine(msg);
                _log.Information(msg);

                if (await InitializeDeviceClient(_retryIntervals))
                {
                    //await BroadcastDeviceRegistration(_deviceClient, _macAddress, _retryIntervals, _log);
                    //await RegisterDeviceBoundMethods();
                }
                else
                {
                    throw new Exception("Corrupt DeviceId / MacAddress value");
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Failed to initialize device client.");
                throw new Exception("Failed to initialize device client.");
            }
        }

        public async Task<bool> InitializeDeviceClient(IEnumerable<TimeSpan> retryIntervals)
        {
            if (!string.IsNullOrWhiteSpace(_deviceId)
                && !string.IsNullOrWhiteSpace(_macAddress))
            {
                var retryPolicy = Policy
                    .Handle<UnauthorizedException>()
                    .WaitAndRetryAsync(
                        retryIntervals,
                        async (ex, waitTime) =>
                        {
                            _log.Warning("DeviceClient was unauthorized ({ErrorMessage}). Attempting to register via the DeviceService...", ex.Message);
                            try
                            {
                                var response = await _registerDeviceRequest(_deviceId)
                                    .PostAsync(null);

                                _log.Information("IoT Hub Registration response was: {Response} - {Reason}",
                                    response.StatusCode, response.ReasonPhrase);
                            }
                            catch (Exception e)
                            {
                                _log.Error(e, "Failed to register the device with IoT Hub.");
                            }

                            _log.Warning("Retrying after {WaitTime:hh\\:mm\\:ss\\.ffff}.", waitTime);
                        });

                await retryPolicy.ExecuteAsync(async () =>
                {
                    await _deviceClient.OpenAsync();
                    await _deviceClient.SendEventAsync(new Message().WithProperties(("message-type", "device-connected")));

                }).ConfigureAwait(false);

                return true;
            }
            else
            {
                _log.Warning("Corrupt Message / DeviceId value: {0},{1}", _deviceId, _macAddress);
                return false;
            }
        }
        private static async Task BroadcastDeviceRegistration(IDeviceClient deviceClient, string macAddress, IEnumerable<TimeSpan> retryIntervals, ILogger log)
        {
            var policy = Policy<bool>
                .HandleResult(false)
                .WaitAndRetryAsync(
                    retryIntervals,
                    (res, time) => log.Warning("BroadcastDeviceRegistration failed on attempt at {Interval} (Success={Result}).", time, res.Result));

            var result = await policy.ExecuteAsync(async () =>
            {
                try
                {
                    log.Information("Sending device-registration message with mac address: {@macAddress}", macAddress);

                    var message = new Message()
                        .WithProperties(
                            ("message-type", "device-registration"));

                    if (!string.IsNullOrWhiteSpace(macAddress))
                    {
                        message.Properties.Add("mac-address", macAddress);
                        message.Properties.Add("device-details", "Fleet Service Engine is the gateway for transferring FSC telematry data to Fleed application.");

                        await deviceClient.SendEventAsync(message);
                        log.Information("Device-registration message sent successfully.");

                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    log.Error(ex, "Failed to send device-registration message.");
                    return false;
                }
            });

            if (result)
            {
                log.Information("Send device-registration - Success.");
            }
            else
            {
                log.Warning("Send device-registration - Fail.");
            }
        }

        private async Task RegisterDeviceBoundMethods()
        {
            foreach (var deviceBoundMethodHandler in _methodHandlers)
            {
                await deviceBoundMethodHandler.Register().ConfigureAwait(false);
            }
        }
    }
    public static class MessageExtensions
    {
        public static Message WithProperties(this Message message, params (string key, string value)[] properties)
        {
            foreach (var (key, value) in properties)
            {
                message.Properties.Add(key, value);
            }
            return message;
        }
    }
}

