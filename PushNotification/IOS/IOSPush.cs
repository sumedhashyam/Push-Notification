using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PushSharp.Core;
using PushSharp.Apple;
namespace PushNotification.IOS
{
    public class IOSPush
    {
        const string APNS_SANDBOX_HOST = "gateway.sandbox.push.apple.com";
        const string APNS_PRODUCTION_HOST = "gateway.push.apple.com";
        const int APNS_PORT = 2195;
        String CertificatePath = "";
        String CertificatePass = "";
        ApnsServerEnvironment ServerEnvironment = ApnsServerEnvironment.Sandbox;
        string host;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="certificatePath">path of .p12 file</param>
        /// <param name="certificatePass">.p12 file password.</param>
        public IOSPush(ApnsServerEnvironment serverEnvironment, string certificatePath, string certificatePass)
        {
            ServerEnvironment = serverEnvironment;
            host = serverEnvironment == ApnsServerEnvironment.Production ? APNS_PRODUCTION_HOST : APNS_SANDBOX_HOST;
            CertificatePath = certificatePath;
            CertificatePass = certificatePass;
        }

        private static bool ValidateServerCertificate(object sender,X509Certificate certificate,X509Chain chain,SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        private byte[] HexToData(string hexString)
        {
            if (hexString == null)
                return null;

            if (hexString.Length % 2 == 1)
                hexString = '0' + hexString; // Up to you whether to pad the first or last byte

            byte[] data = new byte[hexString.Length / 2];

            for (int i = 0; i < data.Length; i++)
                data[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return data;
        }
        /// <summary>
        /// Push sending
        /// </summary>
        /// <param name="userDeviceToken">Device token</param>
        /// <param name="message">Message that will be displayed in notification bar</param>
        /// <param name="custom_Field">Custom fields separated by ';'. For example ID=5;Name=shyam</param>
        public void Send(string userDeviceToken, string message, string custom_Field)
        {                 

            //load certificate
            
            X509Certificate2 clientCertificate = new X509Certificate2(CertificatePath, CertificatePass, X509KeyStorageFlags.MachineKeySet);
            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

            TcpClient client = new TcpClient(host, APNS_PORT);
            SslStream sslStream = new SslStream(
                    client.GetStream(),
                    false,
                    new RemoteCertificateValidationCallback(ValidateServerCertificate),
                    null
            );

            try
            {
                sslStream.AuthenticateAsClient(host, certificatesCollection, SslProtocols.Tls, true);
            }
            catch (AuthenticationException ex)
            {
                client.Close();
                return;
            }

            // Encode a test message into a byte array.
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            writer.Write((byte)0);  //The command
            writer.Write((byte)0);  //The first byte of the deviceId length (big-endian first byte)
            writer.Write((byte)32); //The deviceId length (big-endian second byte)
            
            writer.Write(HexToData(userDeviceToken.ToUpper()));

            String payload = "{\"aps\":{\"alert\":\"" + message + "\",\"badge\":1}";
            String PayloadMess = "";

            List<string> Key_Value_Custom_Field = new List<string>();
            if (string.IsNullOrWhiteSpace(custom_Field) == false)
            {
                List<string> list_Custom_Field = custom_Field.Split(';').ToList();

                if (list_Custom_Field.Count > 0)
                {
                    for (int indx = 0; indx < list_Custom_Field.Count; indx++)
                    {
                        Key_Value_Custom_Field = list_Custom_Field[indx].Split('=').ToList();
                        if (Key_Value_Custom_Field.Count > 1)
                        {
                            if (PayloadMess != "") PayloadMess += ", ";
                            PayloadMess += "\"" + Key_Value_Custom_Field[0].ToString() + "\":\"" + Key_Value_Custom_Field[1].ToString() + "\"";
                        }
                    }
                }
            }

            if (PayloadMess != "")
            {
                payload += ", " + PayloadMess;
            }
            payload += "}";
            writer.Write((byte)0); //First byte of payload length; (big-endian first byte)
            writer.Write((byte)payload.Length); //payload length (big-endian second byte)

            byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
            writer.Write(b1);
            writer.Flush();

            byte[] array = memoryStream.ToArray();
            sslStream.Write(array);
            sslStream.Flush();

            // Close the client connection.
            client.Close();
        }
        public void SendWithJSon(string userDeviceToken,string message,string json, Action<Result> callback)
        {
            try
            {
                var config = new ApnsConfiguration(ServerEnvironment == ApnsServerEnvironment.Sandbox ? ApnsConfiguration.ApnsServerEnvironment.Sandbox : ApnsConfiguration.ApnsServerEnvironment.Production, CertificatePath, CertificatePass);
                var broker = new ApnsServiceBroker(config);
                broker.OnNotificationFailed += (notification, exception) =>
                {
                    callback(new Result { status = "FAIL", message = exception.Message });
                    Console.WriteLine("failed");
                };
                broker.OnNotificationSucceeded += (notification) =>
                {
                    callback(new Result { status = "Success", message = "" });
                    Console.WriteLine("pass");
                };
                broker.Start();

                broker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = userDeviceToken,
                    Payload = JObject.Parse("{ \"aps\" : { \"alert\" : \"" + message + "\" },\"notification\":" + json + " }")
                });


                broker.Stop();
            }
            catch(Exception ex)
            {

                callback(new Result { status = "FAIL", message = ex.Message });
            }
        }
    }
}