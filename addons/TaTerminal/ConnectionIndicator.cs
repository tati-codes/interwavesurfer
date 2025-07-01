#if TOOLS
using Godot;
using System;
using System.Net.Security;
using Taterminal;
[Tool]
public partial class ConnectionIndicator : TextureButton
{
	[Export]
	public Buffer buffer;
  [Export]
  public Listener listener;
  Color red = new Color(200,0,0);
  Color green = new Color(0,200,0);
  Color blue = new Color(1,1,1);
  public void setGreen() => this.Modulate = green;
  public void setBlue() => this.Modulate = blue;
  public override void _Pressed() {
    buffer.consume(new DbgString() {
      name = "Server status: " + (listener.server.IsListening() ? "Online" : "Offline"),
      text = $"Port: {listener.port}\n"
    });
  }
}
#endif