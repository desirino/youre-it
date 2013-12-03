# Getting Started

Photon is a development framework for multiplayer games and applications. It consists of client and server software and this is a client SDK for Mono. Many other client platforms are supported and can communicate cross-platform.

Using the [Photon Cloud](http://cloud.exitgames.com), you can have thousands of games running without ever setting up a server. Alternatively you can download the Photon Server and run it yourself. With the Server SDK you can build your own server logic in C# without handling the low level communication yourself.


## Headstart with Photon Cloud

To get started, we will use the Photon Cloud service. This is without obligation or costs for you but lets us skip the distracting work of setting up a server yourself.

Registration is the same for all platforms and described here:

[http://doc.exitgames.com/photon-cloud/FSteps_PLAIN](http://doc.exitgames.com/photon-cloud/FSteps_PLAIN)

## Particle Demo Setup

The Particle Demo is a simple, code-focused sample that shows some of Photon's "out of the box" features and the api to use. It's GUI is plain and will show just a grid with players as rectangles moving around.

To make it run, get an AppId from the [Photon Cloud Dashboard](https://cloud.exitgames.com/Dashboard), and insert it into the demo. **Open MainActivity.cs and replace the value of field AppId.**

Essentially, the demo is now ready to compile and run on device. Run it multiple times to see clients interact or add multiple "peers" in one app. Each has a separate connection and runs a distinct game "logic".

## The Particle-Demo Code

Aside from reading the documentation and reference, you should check out the code of the samples to learn using Photon. The api is simple but flexible. A lot is client-defined.

GameLogic.cs encapsulates the logic this demos needs. It's built to be included into any game loop and will send and handle events defined for movement and other player info.

The ParticlePlayer.cs and ParticleRoom.cs are actually just really minor extensions of the default features provided by the LoadBalancing API, which is referenced.

## More info

Each of our SDKs includes it's own reference documentation and demos. You should read that in doubt.

We also have a growing number of [online resources in our developer network](http://doc.exitgames.com/photon-cloud).

If any questions remain unanswered [post in our forum](http://forum.exitgames.com).