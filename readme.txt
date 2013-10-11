
This is the readme for the Photon Client SDKs.
(C) Exit Games GmbH 2013



Reference Documentation
----------------------------------------------------------------------------------------------------
Photon-DotNet-Client-Documentation_v3-2-x-y.pdf
http://doc.exitgames.com/photon-cloud
http://doc.exitgames.com/photon-server



Running Loadbalancing Demos
----------------------------------------------------------------------------------------------------
Several of our demos are built for the Photon Cloud. Using the basic Photon Cloud service is free 
and the least effort to get the demos running.

Every game title on the Cloud gets it's own AppId, which must be built into the clients. The demos 
use a property "AppId" in the source files. Find the property AppId definition and set it's value
before you build and run the demos.

More about the Photon Cloud service and "Sign Up" here:
https://cloud.exitgames.com

Find the AppId(s) for your Photon Cloud account as "ID" on your dashboard:
https://cloud.exitgames.com/dashboard


Alternatively you can host a "Photon Cloud" yourself and use any AppId. This is what you need to do:
- Download the Photon Server SDK (it comes as 7zip file or self-extracting exe).
- Unpack-
- Execute PhotonControl.exe.
- Click "Game Server IP Config" and pick a suitable IP address (local network or public).
- This IP must be used in your game as (master) server address. Don't forget the port after ":".
- Click "Loadbalancing (MyCloud)" and "Start as Application".

How to start the server:
http://doc.exitgames.com/photon-server/PhotonIn5Min/#cat-First%20Steps

Download the server SDK here:
http://www.exitgames.com/download/photon


The demos that are compatible with the cloud are in folders named "Cloud", "Loadbalancing" or "LB".



Unity3d Notes
----------------------------------------------------------------------------------------------------
If you don't use Unity3D, skip this chapter.

Currently supported export platforms are: 
    Standalone
    Web (both Windows and MacOS)
    iOS (needs Android Pro Unity license)
    Android (needs iOS Pro Unity license)
    Windows 8 Store and Phone

As of v3.2.1.6 all Unity projects must use ExitGames.Client.Photon.Hashtable. This provides
compatibility with Win 8 RT and Win 8 Phone exports.
Add this to your code (at the beginning=, to resolve the "ambiguous Hashtable" declaration:
using Hashtable = ExitGames.Client.Photon.Hashtable;

Web players do a policy-file request to port TCP 843 before they connect to a remote server. 
The Photon Cloud and Server SDK will handle these requests.
If you host the server, open the additional "policy request" port: TCP 843. If you configure
your server applications, run "Policy Application" for webplayers.


How to add Photon to your Unity project:
1) Copy the Photon .dll somewhere into the "Assets" folder of your project. Avoid dll duplicates!
2) Make sure to have the following line of code somewhere in your scripts, to run it in background:
   Application.runInBackground = true; //without this Photon will loose connection if not focussed
3) Add using Hashtable = ExitGames.Client.Photon.Hashtable; to your scripts.
4) For the iOS builds set
   "iPhone Stripping Level" to "Strip Bytecode" (Edit->Project Settings->Player) and use 
   "DotNet 2.0 subset".
   If your project runs fine in IDE but fails on device, check the "DotNet 2.0 Subset" option!
5) Change the server address in the client. A default of "localhost:5055" won't work on device.



Windows Phone Notes
----------------------------------------------------------------------------------------------------
To run demos on a smartphone or simulator, you need to setup your server's network address. 

We assume you run a Photon Server on the same development machine, so by default, our demos have a 
address set to "127.0.0.1:5055" and use UDP.
Demos for LoadBalancing or the Cloud will be set to: "app.exitgamescloud.com:5055" and use UDP.

Search the code and replace those values to run the demos on your own servers or in a simulator.
The Peer.Connect() method is usually used to set the server's address.

A smartphone has it's own IP address, so 127.0.0.1 will contact the phone instead of the machine 
that also runs Photon.



Xamarin Notes
----------------------------------------------------------------------------------------------------
Edit MainActivity.cs with your own AppId (approx. line 33). Read "Running Loadbalancing Demos".
Alternatively change the ServerAddress to a Photon Server you are hosting. Don't forget the ":port".

Our library dlls are fully Xamarin.iOS, Xamarin.Android and Xamarin.mac compatible, 
but the projects to build them are not. So you can't reference the LoadBalancingApi project 
directly.
Instead, we reference the dlls to minimize the number of projects to maintain and potential for 
desaster.

The demo particle links directly to some of the source files of demo-particle-logic for similar 
reasons.

Let us know if our workflow doesn't work for you.




Playstation Mobile Notes
----------------------------------------------------------------------------------------------------
The demo by default connects to the EU Photon Cloud servers. You only need to register to run your 
own application (game) on it. This gives you the AppId which must be inserted into:
demo-particle-psm\AppMain.cs