﻿using System;
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
            var path = "path of .p12 file";
            IOSPush iosPush = new IOSPush(ApnsServerEnvironment.Production/*ApnsServerEnvironment.Sandbox in case of development*/, path, "Password of p12 file");
        
              var json = "{\"ID\":\"121\",\"Name\":\"Shyam\",\"notificationActions\":[{\"NotificationName\":\"Add to reminder\",\"NotificationActionEntityValues\":[{\"NotificationEntityName\":\"Title\",\"NotificationEntityValue\":\"Test\"},{\"NotificationEntityName\":\"DateTime\",\"NotificationEntityValue\":\"Pqr-28-2016 8:00:00\"}]},{\"NotificationName\":\"Add to calendar\",\"NotificationActionEntityValues\":[{\"NotificationEntityName\":\"Title\",\"NotificationEntityValue\":\"Title\"},{\"NotificationEntityName\":\"Location\",\"NotificationEntityValue\":\"Location\"},{\"NotificationEntityName\":\"All day\",\"NotificationEntityValue\":\"N\"},{\"NotificationEntityName\":\"Start DateTime\",\"NotificationEntityValue\":\"Oct-30-2016 8:00:00\"},{\"NotificationEntityName\":\"End DateTime\",\"NotificationEntityValue\":\"Oct-30-2016 17:00:00\"},{\"NotificationEntityName\":\"Alert\",\"NotificationEntityValue\":\"None\"},{\"NotificationEntityName\":\"Show As\",\"NotificationEntityValue\":\"None\"},{\"NotificationEntityName\":\"Note\",\"NotificationEntityValue\":\"Some notes here\"}]}]}";
             //Regular push notification: 4KB (4096 bytes) based on HTTP/2-based APNs provider API
			 iosPush.SendWithJSon("device token", "Test push send by Shyam Agarwal", json,new Action<Result>(x => {
                 Console.WriteLine(x.status);
             }));
			 
			 //Regular push notification: 2KB (2048 bytes) based on Legacy APNs binary interface
             iosPush.Send("device token", "Test push send by Shyam Agarwal", "ID=121;Name=Shyam");
            
        }
    }
}