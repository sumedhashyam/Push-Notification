using Newtonsoft.Json.Linq;
using PushNotification.IOS;
using PushSharp.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNotification.Android
{
    public class AndroidPush
    {
        String SenderID = "";
        String AuthToken = "";

        public AndroidPush(string senderID,string authToken)
        {
            SenderID = senderID;
            AuthToken = authToken;
        }
        /// <summary>
        /// Send push message with custom json.
        /// </summary>
        /// <param name="userGCMKey">GCM registration key</param>
        /// <param name="collapseKey">Unique Id that identify the push desination page.</param>
        /// <param name="json">JSON for extra fields including message</param>
        /// <param name="callback">callback function</param>
        public void SendWithJSon(string userGCMKey, string collapseKey,string message, string json, Action<Result> callback)
        {
            try
            {
                var config = new GcmConfiguration(SenderID, AuthToken, null);
                var broker = new GcmServiceBroker(config);
                broker.OnNotificationFailed += (notification, exception) => {
                    callback(new Result { status = "FAIL", message = exception.Message });
                };
                broker.OnNotificationSucceeded += (notification) => {

                    callback(new Result { status = "Success", message = "" });
                };

                broker.Start();
                broker.QueueNotification(new GcmNotification
                {
                    RegistrationIds = new List<string> {
                        userGCMKey
                    },
                    Data = JObject.Parse("{\"message\":\"" + message + "\",\"payload\":" + json + "}"),
                    CollapseKey = collapseKey,
                    TimeToLive = 108,
                    DelayWhileIdle = true
                });
                broker.Stop();
        }
            catch (Exception ex)
            {

                callback(new Result { status = "FAIL", message = ex.Message });
            }
        }
    }
}
