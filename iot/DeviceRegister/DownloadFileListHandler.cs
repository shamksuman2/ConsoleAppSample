//using Microsoft.Azure.Devices.Client;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace DeviceRegisteration
//{
//    public class DownloadFileListHandler
//    {
//        private readonly DeviceClient _deviceClient;
//        private readonly IDownloadService _downloadservice;
//        private readonly AppSettings _appSettings;
//        private readonly ILogger<DownloadFileListHandler> _logger;
//        private readonly IValidator<DownloadRequestMessage> _downloadRequestValidator;
//        private DownloadResponseMessage _downloadResponse;
//        private MessageStatus _messageStatus;

//        public DownloadFileListHandler(DeviceClient deviceClient, IDownloadService downloadservice, AppSettings appSettings, ILogger logger, IValidator<DownloadRequestMessage> downloadRequestValidator)
//        {
//            _deviceClient = deviceClient;
//            _downloadservice = downloadservice;
//            _appSettings = appSettings;
//            _logger = logger;
//        }
//        public async Task Register()
//        {
//            if (_deviceClient == null)
//            {
//                _logger.LogCritical("Needs to initialize deviceClient FIRST");
//                return;
//            }
//            await _deviceClient.SetMethodHandlerAsync("DownloadDeltaContentFilesList", DownloadDeltaContentFilesList, null).ConfigureAwait(false);
//        }
//    }
//}
