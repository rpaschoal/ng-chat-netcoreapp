# ng-chat-netcoreapp
A demo of ng-chat using ASP.NET core and Azure SignalR. [The published version can be found here](https://ng-chat.azurewebsites.net).

### Running this project

* Clone this repository
* Create an Azure SignalR resource under your Azure account (The free tier will do just fine for the demo)
* Add a user secret with your Azure SignalR resource with the following key `Azure:SignalR:ConnectionString` (`dotnet user-secrets set Azure:SignalR:ConnectionString "<Your connection string>"`)
* Open the project solution using Visual Studio 2017 or higher (You can also run them separately without Visual Studio using the dotnet CLI with `dotnet run`)
* Run both projects in the solution (Make sure you're running it with Kestrel and that the ports are matching with the launch settings)
* Test joining a room with different browsers (or a incognito window) and exchange messages.

### Relevant files
* [Offline bot adapter](https://github.com/rpaschoal/ng-chat-netcoreapp/blob/master/NgChatClient/ClientApp/src/app/demo-adapter.ts)
* [Signalr adapter](https://github.com/rpaschoal/ng-chat-netcoreapp/blob/master/NgChatClient/ClientApp/src/app/signalr-adapter.ts)
* [Signalr chat hub](https://github.com/rpaschoal/ng-chat-netcoreapp/blob/master/NgChatSignalR/ChatHub.cs)
### Considerations
This project is just a demo for [ng-chat](https://github.com/rpaschoal/ng-chat). This code is not production ready.
