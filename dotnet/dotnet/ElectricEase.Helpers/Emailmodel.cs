using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricEase.Helpers
{
        public class EmailLog
        {
            public string EmailTo { get; set; }
            public string EmailCC { get; set; }
            public string Subject { get; set; }
            public string BodyContent { get; set; }
            public string Status { get; set; }
            public bool isSent { get; set; }
            public DateTime SentDate { get; set; }
            public string SentBy { get; set; }
        }

        public class EmailAttachments
        {
            public string AttachmentName { get; set; }
            public byte[] Attachment { get; set; }
            public int EmailId { get; set; }


        }
    }

