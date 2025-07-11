using Godot;
using System;
using System.Collections.Generic;
using OBus;

[GlobalClass]
public partial class Bus : Node {
  public static Bus instance;
  [Export]
  Client busClient {get; set;}
  [Export]
  bool productionMode {get; set;} = false;
  public override void _Ready() {
    if (!productionMode) {
      busClient.subscribe(this);
      busClient.connect();
    } else {
      busClient.isconnected = true;
    }
  }
  private System.Collections.Generic.Dictionary<Type, Func<Object>> _mapping = new Dictionary<Type, Func<Object>>() { };
  private T GetEvent<T, TArgs>()
      where T : TEvent<TArgs>, new()
      where TArgs : Args
  {
    if (_mapping.ContainsKey(typeof(T))) {
        return _mapping[typeof(T)]() as T;
    }
    var presEvent = new T();
    _mapping.Add(typeof(T), () => presEvent);
    return presEvent;
  }
  public Subscription Subscribe<T, TArgs>(Action<TArgs> action)
    where T : TEvent<TArgs>, new()
    where TArgs : Args 
  {
    var presEvent = GetEvent<T, TArgs>();
    return presEvent.Subscribe(action);
  }
  public void Publish<T, TArgs>(TArgs args, bool propagate = true) where T : TEvent<TArgs>, new()
    where TArgs : Args
  {
    var presEvent = GetEvent<T, TArgs>();
    presEvent.Publish(args);
    if (propagate) {
      ExternalLogger<T, TArgs>(args);
    }
  }


  private void ExternalLogger<T, TArgs>(TArgs args)
    where T : TEvent<TArgs>, new()
    where TArgs : Args
  {
    var presEvent = GetEvent<T, TArgs>();
    DbgString dbg = null;
    if (presEvent is BgLog || presEvent is TLog) return;
    else {
      dbg = DbgString.createFrom<T, TArgs>(presEvent, args);
      Publish<BgLog, DbgString>(dbg);
    }
  }
}