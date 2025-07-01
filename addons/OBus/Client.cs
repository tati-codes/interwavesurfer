#if TOOLS 
using Godot;
using System;
using OBus;
using System.Threading.Channels;
using System.Threading;
using System.Threading.Tasks;
[GlobalClass]
public partial class Client : Node
{
    [Export]
    string Url {get; set;} = "ws://127.0.0.1:2120";
    WebSocketPeer socket = new WebSocketPeer();
    WebSocketPeer.State state = WebSocketPeer.State.Closed;
    private readonly Channel<DbgString> _channel = Channel.CreateUnbounded<DbgString>();
    public ChannelWriter<DbgString> _writer { get => _channel.Writer; }
    public ChannelReader<DbgString> _reader { get => _channel.Reader; }
    public readonly CancellationTokenSource _cancellation = new();
    private readonly TaskCompletionSource<bool> connectionReady = new();
    public bool isconnected = false;
    public override void _Ready() {
      _ = Task.Run(process);
    }
    public async Task OnOutboundEvent(DbgString dbgStr) => await _writer.WriteAsync(dbgStr);
    void ConnectionMade() { 
      isconnected = true;
      connectionReady.SetResult(true);
    }
    void ConnectionClosed() {
      isconnected = false;
      connectionReady.SetResult(false);
    }
    void ConnectionFailed(Exception ex) => connectionReady.SetException(ex);
    private async Task process() {
      try {
        await connectionReady.Task;
        await foreach (DbgString str in _reader.ReadAllAsync(_cancellation.Token)) {
          try {
            var exported = str.export();
            send(exported);
          } catch (Exception e) {
            GD.PrintErr($"Couldn't send event '{str.name}': {e.Message}");
          }
        }
      } catch (Exception ex) {
        GD.PrintErr($"No idea, see this: {ex.Message}");
      }
    }
    public void subscribe(Bus bus) {
      bus.Subscribe<BgLog, DbgString>(async args => await OnOutboundEvent(args));
    }
    public Godot.Error connect() {
      var e = socket.ConnectToUrl(Url); 
      state = socket.GetReadyState();
      return e;
    }
    public void close() {
      socket.Close(1000, "");
      state = socket.GetReadyState();
    }
    public void send(string message) {
      if (state == WebSocketPeer.State.Open) {
        try {
          socket.SendText(message);
        } catch (Exception e) {
            GD.PrintErr($"Couldn't send event: {e.Message}");
        }
      } else {
          GD.Print("Socket not ready to send");
      }
    }
    public void poll() {
      if (socket.GetReadyState() != WebSocketPeer.State.Closed) {
        socket.Poll();
      }
      var newest = socket.GetReadyState();
      if (state != newest) {
        state = newest;
        if (state == WebSocketPeer.State.Open) {
            ConnectionMade();
        } else if (state == WebSocketPeer.State.Closed) {
            ConnectionClosed();
        }
      }
    }
    public override void _Process(double delta) {
      if (!isconnected) {
        poll();
      }
    }
  public override void _ExitTree()
  {
    close();
  }
}


#endif