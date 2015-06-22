using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.ViewModels
{
    public class MsgNotification
    {
        public string MessageType { get; set; }
        public string MessageContent { get; set; }

        public MsgNotification(string msgStr, string type = MsgType.Info)
        {
            this.MessageContent = msgStr;
            this.MessageType = type;
        }
    }
}