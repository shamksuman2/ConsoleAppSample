using System.Diagnostics.CodeAnalysis;

namespace Dover.Fleet.Entity
{
    [ExcludeFromCodeCoverage]
    /// <summary>
    /// Message structure for communication with clients.
    /// </summary>
    public class NotificationItem
    {
        ///// <summary>
        ///// email
        ///// </summary>
        //public string RecipientName { get; set; }

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// email message
        /// </summary>
        public string EmailMessage { get; set; }

        /// <summary>
        /// sms text
        /// </summary>
        public string SmsText { get; set; }

        /// <summary>
        /// email message
        /// </summary>
        public string AlertClearedEmailMessage { get; set; }

        /// <summary>
        /// sms text
        /// </summary>
        public string AlertClearedSmsText { get; set; }

        /// <summary>
        /// AlertID
        /// </summary>
        public string AlertID { get; set; }

        /// <summary>
        /// StatusType
        /// </summary>
        public string StatusType { get; set; }

    }
}