#if TOOLS
using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using Taterminal;
[Tool]
public partial class Listener : Node
{
  [Export]
  public ConnectionIndicator indi {get; set;}
  [Export]
	public ushort port { get; set;} = 2129;
	public TcpServer server = new ();
	List<PendingPeer> pendingPeers = new();
  ulong handshake_timeout = 3000;
	bool use_tls = false;
	bool refuse_new_connections = false;
	int max_retry = 5;
	int current_try = 0;
	WebSocketPeer client;
	[Export]
	public Buffer buffer { get; set; }
	public override void _Ready() {
        listen();	
	}
	public Godot.Error listen() {
		current_try++;
		if (!server.IsListening())
		{
			return server.Listen(port, "127.0.0.1");
		} else {
			buffer.consume(Text.error("Server already listening;"));
      return Godot.Error.AlreadyExists;
		}
	}
	public void stop() {
		server.Stop();
		pendingPeers.Clear();
	}
	public WebSocketPeer createPeer() {
		var ws = new WebSocketPeer();
		return ws;
	}
	public void poll() {
		if (!server.IsListening()) {
			return;
		}
		if (!refuse_new_connections && server.IsConnectionAvailable()) {
			var conn = server.TakeConnection();
			if (conn != null) {
				pendingPeers.Add(new PendingPeer(conn));
				refuse_new_connections = true;
			}
		}
		List<PendingPeer> toremove = new List<PendingPeer>();

		if (pendingPeers.Count > 0) {
			foreach (var peer in pendingPeers.ToList()) {
				if (!connect_pending(peer)) {
					if ((peer.connect_time + handshake_timeout) < Time.GetTicksMsec()) {
						toremove.Add(peer);
					}
					continue;
				}
				toremove.Add(peer);
			}
			toremove.Clear();
		}

		if (client != null) {
        client.Poll();
        if (client.GetReadyState() != WebSocketPeer.State.Open) {
          indi.setBlue();
          client = null;
          refuse_new_connections = false;
          return;
        }
        while (client.GetAvailablePacketCount() > 0) {
          var message = getMessage(client);
          if (message != null) {
            interpret(message);
          }
        }
      }
    }
  
	//TODO make this differentiate between dbgstring and Text etc
  void interpret(string msg) {
    try {
      DbgString interpreted = DbgString.import(msg);
      buffer.consume(interpreted);
    } catch (Exception e) {
      buffer.consume(Text.error("Message error: " + e.Message));
    }
  }
	public string getMessage(WebSocketPeer socket) {
		if (socket.GetAvailablePacketCount() < 1) {
			return null;
		}
		var pkt = socket.GetPacket();
		if (socket.WasStringPacket()) {
			return pkt.GetStringFromUtf8();
		} else {
			return null;
		}
	}
	public bool connect_pending(PendingPeer pendingPeer) {
		if (pendingPeer.ws != null) {
			pendingPeer.ws.Poll();
			var state = pendingPeer.ws.GetReadyState();
			if (state == WebSocketPeer.State.Open) {
				indi.setGreen();
				client = pendingPeer.ws;
				pendingPeers.Clear();
        return true;
			} else if (state == WebSocketPeer.State.Connecting) {
				return true;
			}
			  return false; //connecting
		} else if (pendingPeer.tcp.GetStatus() != StreamPeerTcp.Status.Connected) {
        return true; //??
		} else if (use_tls == false) {
        pendingPeer.ws = createPeer();
        pendingPeer.ws.AcceptStream(pendingPeer.tcp);
        return false; //connection pending
		} else {
        throw new Exception("something's fucked up here");
		}
	}
	public override void _Process(double delta) {
		if (server.IsListening()) {
			poll();
			current_try = 0;
		} else if (current_try < max_retry) {
			this.listen();
		} else {
			return;
		}
	}

  //FIXME merge this upstream
  public override void _ExitTree()  {
    stop();
  }
}
namespace Taterminal
{
	public class PendingPeer
	{
		public ulong connect_time;
		public StreamPeerTcp tcp;
		public StreamPeer connection;
		public WebSocketPeer ws;

		public PendingPeer (StreamPeerTcp p_tcp)
		{
			tcp = p_tcp;
			connection = p_tcp;
			connect_time = Time.GetTicksMsec();
		}
	}
    public class ClientConnected : TEvent<NArgs> { }
	public class ClientDisconnected: TEvent<NArgs> { }
}

#endif