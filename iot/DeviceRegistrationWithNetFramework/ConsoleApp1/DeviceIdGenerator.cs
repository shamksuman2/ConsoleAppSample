using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Serilog;

namespace DeviceRegWithNetFramework
{
    public class DeviceIdGenerator : IDeviceIdGenerator
    {
        private readonly ILogger _log;
        private readonly X509Certificate2 _certificate;
        private readonly PlatformID _platform;
        private Guid _generateDeviceId = Guid.Empty;

        private readonly string _prefix;
        private readonly string _extensionName;

        public DeviceIdGenerator(ILogger log, X509Certificate2 certificate, PlatformID platform)
        {
            _log = log;
            _certificate = certificate;
            _platform = platform;

            _prefix = string.Equals(_platform.ToString(), "Win32NT") ? "DNS Name=" : "DNS:";
            _extensionName = string.Equals(_platform.ToString(), "Win32NT") ? "Subject Alternative Name" : "X509v3 Subject Alternative Name";
        }

        public Guid GenerateDeviceId(int uuIdNumber)
        {
            try
            {
                //_log.Information($"{nameof(GenerateDeviceId)} : started.");
                var extension = _certificate.Extensions.Cast<X509Extension>().FirstOrDefault(e => e.Oid.FriendlyName.Equals(_extensionName, StringComparison.InvariantCultureIgnoreCase));

                if (extension != null)
                {
                    var encData = new AsnEncodedData(extension.Oid, extension.RawData);
                    var alternateSubjectNameContent = encData.Format(true);
                    var lines = alternateSubjectNameContent.Split(new[] { _prefix }, StringSplitOptions.RemoveEmptyEntries);

                    var uuid = Guid.Parse(lines[uuIdNumber].Trim().TrimEnd(','));
                    // Incase of Unix Platform the Alternate SubjectNames are separated by ','
                   // _log.Information($"{nameof(GenerateDeviceId)} : completed and the device id : {uuid}.");
                    return uuid;
                }
                else
                {
                    throw new InvalidOperationException($"{nameof(GenerateDeviceId)}: {_extensionName} extension not found in the certificate");
                }
            }
            catch (Exception ex)
            {
                //_log.Error($"{nameof(GenerateDeviceId)}: Unable to extract the certificate information.", ex);
                throw ex;
            }
        }
    }
}