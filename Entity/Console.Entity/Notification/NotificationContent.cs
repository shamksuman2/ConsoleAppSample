using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dover.Fleet.Entity
{
    [ExcludeFromCodeCoverage]

    public class NotificationContent
    {
        /// <summary>
        /// Gets or sets the emails.
        /// </summary>
        /// <value>
        /// The emails.
        /// </value>
        public List<string> emails { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public Message message { get; set; }

        /// <summary>
        /// Gets or sets the phones.
        /// </summary>
        /// <value>
        /// The phones.
        /// </value>
        public List<string> phones { get; set; }

        public List<string> users { get; set; }
    }

    public class DevideData
    {
        public string deviceId { get; set; }
        public string deviceType { get; set; }
        public string siteId { get; set; }
    }


    public class Message
    {

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int Priority { get; set; }

        public List<string> fileUrls { get; set; }
        public DevideData metadata { get; set; }
    }




}
