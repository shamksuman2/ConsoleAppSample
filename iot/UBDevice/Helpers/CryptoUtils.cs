namespace DeviceRegistration.Library
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    //using Serilog;
    public static class CryptoUtils
    {
        public static byte[] ComputeMD5Hash(byte[] input)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(input);
            }
        }

        public static X509Certificate2 LoadCertificate(string commonName, string folderStore = null)
        {
            X509Certificate2 result = null;

            if (Directory.Exists(folderStore))
            {
                //Log.Logger.Information("Attempting to locate certificate with CN={CommonName} from path '{Path}'", commonName, folderStore);

                X509Certificate2 LoadCertificateSafe(string path)
                {
                    try
                    {
                        return new X509Certificate2(path);
                    }
                    catch (Exception ex)
                    {
                        //Log.Logger.Warning("Certificate at path '{Path}' could not be loaded because '{Reason}'", path, ex.Message);
                        return null;
                    }
                }

                var certCollection = new X509Certificate2Collection(
                    Directory.GetFiles(folderStore, "*.pfx")
                        .Select(LoadCertificateSafe)
                        .Where(cert => cert != null)
                        .ToArray());

                result = FindCertificate(commonName, certCollection);
            }
            else
            {

                var macAddress = MacAddressAccessor.GetMacAddress().ToLower();
                result = (LoadCertificateFromStore(commonName, StoreName.My, StoreLocation.CurrentUser)
                         ?? LoadCertificateFromStore(commonName, StoreName.My, StoreLocation.LocalMachine))
                         ?? LoadCertificateFromStore(macAddress, StoreName.My, StoreLocation.LocalMachine, X509FindType.FindBySerialNumber);
            }

            if (result == null)
            {
                //Log.Logger.Warning("Unable to locate a certificate with CN={CommonName}", commonName);
            }
            else
            {
                //Log.Logger.Information("Certificate located: Subject: '{Subject}', Thumbprint: {Thumbprint}", result.Subject, result.Thumbprint);
            }

            return result;
        }
        
        private static X509Certificate2 LoadCertificateFromStore(string commonName, StoreName storeName, StoreLocation storeLocation, X509FindType x509FindType = X509FindType.FindBySubjectName)
        {
            //Log.Logger.Information($"Attempting to locate certificate by {x509FindType} = '{commonName}' from store '{storeLocation}/{storeName}'");
            using (var store = new X509Store(storeName, storeLocation))
            {
                store.Open(OpenFlags.ReadOnly);
                return FindCertificate(commonName, store.Certificates, x509FindType);
            }
        }

        private static X509Certificate2 FindCertificate(string commonName, X509Certificate2Collection collection, X509FindType x509FindType = X509FindType.FindBySubjectName)
            => collection
            .Find(x509FindType, commonName, false)
                .Cast<X509Certificate2>()
                .FirstOrDefault();
    }
}
