# Push-Notification
Send push notifications to mobile devices through APNS in .NET.

Simple c# libray using which you can send Push notificaiton to iOS and android device. This library is based on [PushSharp](https://github.com/Redth/PushSharp)

Add .p12 file in application as we have to pass path of the file when calling library. (For iOS only)

### APNS Sample Usage based on HTTP/2-based APNs provider API
```csharp
var path = "path of .p12 file";
IOSPush iosPush = new IOSPush(ApnsServerEnvironment.Production/*ApnsServerEnvironment.Sandbox in case of development*/, path, "Password of p12 file");

//Complex json
var json = "{\"ID\":\"121\",\"Name\":\"Shyam\",\"any_array\":[{\"field 1\":\"field 1 value\",\"sub array\":[{\"field 1\":\"field 1 value\",\"field 2\":\"field 2 value\"}]}]}";

//Regular push notification: 4KB (4096 bytes) based on HTTP/2-based APNs provider API
iosPush.SendWithJSon("device token", "Test push send by Shyam Agarwal", json, new Action<Result>(x => {
  Console.WriteLine(x.status);
}));
```

### APNS Sample Usage based on Legacy APNs binary interface
```csharp
var path = "path of .p12 file";
IOSPush iosPush = new IOSPush(ApnsServerEnvironment.Production/*ApnsServerEnvironment.Sandbox in case of development*/, path, "Password of p12 file");

//Regular push notification: 2KB (2048 bytes) based on Legacy APNs binary interface
iosPush.Send("device token", "Test push send by Shyam Agarwal", "ID=121;Name=Shyam");
```


### GCM push notification (Android)
```csharp
AndroidPush androidPush = new AndroidPush("GCM Sender ID", "API key");

var json = "{\"ID\":\"121\",\"Name\":\"Shyam\",\"any_array\":[{\"field 1\":\"field 1 value\",\"sub array\":[{\"field 1\":\"field 1 value\",\"field 2\":\"field 2 value\"}]}]}";

androidPush.SendWithJSon("GCM device key", "PAGE 1", "Test push send by Shyam Agarwal", json, new Action<Result>(x => {
	Console.WriteLine(x.status);
}));
```
License
-------
You don't need any license for my code but please check license section of [PushSharp](https://github.com/Redth/PushSharp) 