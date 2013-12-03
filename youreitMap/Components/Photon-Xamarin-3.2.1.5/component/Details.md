Photon is a development framework for multiplayer games and applications. It consists of client and server software and this is a client SDK for Mono. Many other client platforms are supported and can communicate cross-platform.

This client libary package supports Xamarin.Android and Xamarin.iOS. The package includes a demo, readme, reference documentation and a change log.

Your clients can either connect to the Photon Cloud (free for development) or you download and run your own Photon instance.

Please refer to the Getting Started document and the included reference documentation for further details.



### Some Examples

To connect, call:

```csharp
using ExitGames.Client.Photon;

public class MyClient : IPhotonPeerListener
{
	LoadBalancingPeer peer;

    public bool Connect()
    {
		// A LoadBalancingPeer lets you connect and call operations on the server. Callbacks go to "this" listener instance and use UDP
		peer = new LoadBalancingPeer(this, ConnectionProtocol.Udp);
        if (peer.Connect("app.exitgamescloud.com:port", AppId))
        {
            return true;
        }

        // connect might fail right away if the address format is bad, e.g.
        return false;
    }
}
```

To get results, implement the IPhotonPeerListener interface:

```csharp
public interface IPhotonPeerListener
{
    void DebugReturn(DebugLevel level, string message);
    void OnOperationResponse(OperationResponse operationResponse);
    void OnStatusChanged(StatusCode statusCode);
    void OnEvent(EventData eventData);
}
```

To create a room, call:

```csharp
// being connected, you can call:
// OpCreateRoom(string roomName, bool isVisible, bool isOpen, byte maxPlayers, Hashtable customGameProperties, string[] propsListedInLobby)
// custom properties are named by you and their value is synced with anyone joining the room
peer.OpCreateRoom(roomNameString, true, true, 0, customPropertiesOfRoom, customPropsShownInLobby);
```

To join a random room, call:

```csharp
// join random rooms easily, filtering for specific room properties, if needed
Hashtable expectedCustomRoomProperties = new Hashtable();
expectedCustomRoomProperties["map"] = 1;    // custom props can have any name but the key must be string
peer.OpJoinRandomRoom(expectedCustomRoomProperties, (byte)expectedMaxPlayers);
```

To send events to others in same room, call:

```csharp
byte eventCode = 1; // make up event codes at will
Hashtable evData = new Hashtable();	// put your data into a key-value hashtable
bool sendReliable = false; // send something reliable if it must arrive everywhere
byte channelId = 0; // for advanced sequencing. can be 0 in most cases
peer.OpRaiseEvent(eventCode, evData, sendReliable, channelId);
```