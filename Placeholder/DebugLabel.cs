using Godot;
using System;
using OBus;
using System.Collections.Generic;
using System.Linq;
public partial class DebugLabel : Label {
	public Bus bus;

  public Dictionary<string, string> tracking = new();
	public override void _Ready() {
    bus = GetNode<Bus>("/root/bus");
    bus.Subscribe<TatiDebug.DisplayDebugScreen, TatiDebug.DebugVar>(processDebugVars); 
	}
  void processDebugVars(TatiDebug.DebugVar args) {
    var (name, details) = args;
    if (tracking.ContainsKey(name)) {
      tracking[name] = details;
    } else {
      tracking.Add(name, details);
    }
    update();
  }

  void update() {
    this.Text = "";
    string result = "";
    foreach ((string name, string deets) in tracking) {
      result += $"{name}: {deets}\n";
    }
    this.Text = result;
  }

}

namespace TatiDebug {
  public class DisplayDebugScreen : TEvent<DebugVar> {}
  public class DebugVar : Args {
    public string Name {get; set;}
    public string Details {get; set;}
    public DebugVar(string _n, string _d) {
      Name = _n;
      Details = _d;
    }
    public DebugVar(string _n, Object o) {
      Name = _n;
      Details = o.ToString();
    }
    public void Deconstruct(out string name, out string details) {
      name = Name;
      details = Details;
    }
  }
}
