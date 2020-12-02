namespace DeviceRegisteration
{

    using Flurl.Http.Configuration;
    using System;
    using System.Security.Cryptography.X509Certificates;
    using System.Net.Http;
    using System.Security.Authentication;
    using System.Net.Security;
    using System.Net;
    using Serilog;

    public class X509ClientFactory : DefaultHttpClientFactory
    {
        private readonly X509Certificate2 _certificate;
        private readonly ILogger _log;

        public X509ClientFactory(X509Certificate2 certificate, ILogger log)
        {
            _certificate = certificate;
            _log = log;
        }

        public override HttpMessageHandler CreateMessageHandler()
        {
            var handler = new HttpClientHandler
            {
                SslProtocols = SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = (request, cert, chain, errors) =>
                {
                    _log.Information("Validating X509 Certificate for server at {RequestUri}{NewLine}Certificate: {Cert}{NewLine}Chain: {Chain}{NewLine}Errors: {Errors}",
                        request.RequestUri,
                        Environment.NewLine,
                        cert,
                        Environment.NewLine,
                        chain.ChainStatus,
                        Environment.NewLine,
                        errors);
                    return request.RequestUri.IsLoopback || errors == SslPolicyErrors.None;
                }
            };

            if (!string.IsNullOrWhiteSpace(System.Environment.GetEnvironmentVariable("http_proxy")))
            {
                handler.Proxy = WebRequest.DefaultWebProxy;
                handler.UseProxy = true;
            }

            handler.ClientCertificates.Add(_certificate);
            return handler;
        }
    }

}