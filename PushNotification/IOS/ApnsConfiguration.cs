using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotification.IOS
{
    public enum ApnsServerEnvironment
    {
        Sandbox,
        Production
    }
    public class Result
    {
        public string status { get; set; }
        public string message { get; set; }
    }
}
