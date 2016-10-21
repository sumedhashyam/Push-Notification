using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushNotification.IOS;

namespace TestApplicaiton
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = "D:/projects/PushNotification/TestApplicaiton/Alertify Production.p12";//path of .p12 file  
            IOSPush iosPush = new IOSPush(ApnsServerEnvironment.Production, path, "1234");
        
              var json = "{\"notificationName\":\"Name\",\"notificationSubject\":\"Subject\",\"notificationBody\":\"Body\",\"sendToUsers\":\"ashkot09@gmail.com,shraddhamulay@gmail.com\",\"scheduledDate\":\"Oct-28-2016 8:00:00\",\"notificationTypeID\":7,\"notificationActions\":[{\"NotificationName\":\"Add to reminder\",\"NotificationActionEntityValues\":[{\"NotificationEntityName\":\"Title\",\"NotificationEntityValue\":\"Test\"},{\"NotificationEntityName\":\"DateTime\",\"NotificationEntityValue\":\"Pqr-28-2016 8:00:00\"}]},{\"NotificationName\":\"Add to calendar\",\"NotificationActionEntityValues\":[{\"NotificationEntityName\":\"Title\",\"NotificationEntityValue\":\"Title\"},{\"NotificationEntityName\":\"Location\",\"NotificationEntityValue\":\"Location\"},{\"NotificationEntityName\":\"All day\",\"NotificationEntityValue\":\"N\"},{\"NotificationEntityName\":\"Start DateTime\",\"NotificationEntityValue\":\"Oct-30-2016 8:00:00\"},{\"NotificationEntityName\":\"End DateTime\",\"NotificationEntityValue\":\"Oct-30-2016 17:00:00\"},{\"NotificationEntityName\":\"Alert\",\"NotificationEntityValue\":\"None\"},{\"NotificationEntityName\":\"Show As\",\"NotificationEntityValue\":\"None\"},{\"NotificationEntityName\":\"Note\",\"NotificationEntityValue\":\"Some notes here\"}]}]}";
             iosPush.SendWithJSon("53ae9aa1bcb790027c5ca816a2e9648d04ceb2e1d9dfae4551d52ba4414b310a", "Test push send by Shyam Agarwal", json,new Action<Result>(x => {
                 Console.WriteLine(x.status);
             }));
            // iosPush.SendWithJSon("3cd8e888c3f7f75b345aa294db083992e79c9059e66772095d6ca04b2a52d455", "Test push send by Shyam Agarwal", json);
            //  iosPush.Send("53ae9aa1bcb790027c5ca816a2e9648d04ceb2e1d9dfae4551d52ba4414b310a", "Test push send by Shyam Agarwal", "ID=121;Name=Shyam");
            //  iosPush.Send("1834a6920b69f56fd122dc0032dce935f1d1071f875cc492b2db5de1ca91291e", "Test push send by Shyam Agarwal", "ID=121;Name=Shyam");
        }
    }
}
