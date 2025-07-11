//TODO remove if tools preprocessor for OBus
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using OBus;

public partial class Bus : Node {
  public void Log(string msg) => this.Publish<Log, Text>(new Text(msg)); 
  public void Warn(string msg) => this.Publish<Warn, Text>(Text.warn(msg));
  public void LogAsTag(string msg, string tag) => this.Publish<TLog, DbgString>(new DbgString() { name = "", text = msg, tag = tag});
  public void Error(string msg) => this.Publish<OBus.Error, Text>(Text.error(msg));
  public void Subscribe<T>(Action<NArgs> action)
      where T : TEvent<NArgs>, new() => Subscribe<T, NArgs>(action);
  public void Publish<T, TArgs>() where T : TEvent<TArgs>, new() 
    where TArgs : Args, new() => Publish<T, TArgs>(new TArgs());
  public void Publish<T>(LOG_LEVEL _level = LOG_LEVEL.MINIMAL) where T : TEvent<NArgs>, new() => Publish<T, NArgs>(new NArgs() { level = _level});
  public void Error(string msg,  Exception ex) => Error($"[b]{msg}[/b]: " + ex.Message + "\n" + ex.StackTrace);
  public void Log(params string[] strings) => this.Publish<Log, Text>(new Text(String.Join(" ", strings))); 
  public void Warn(params string[] strings) => this.Publish<Warn, Text>(Text.warn(String.Join(" ", strings)));
  public void Error(params string[] strings) => this.Publish<OBus.Error, Text>(Text.error(string.Join(" ", strings)));
  public void Count(string label) => Publish<Count, Text>(Text.count(label));

  /// <summary>
  /// For internal, high-frequency events that don't need to be logged.
  /// </summary>
  public void IPub<T, TArgs>(TArgs args) where T : TEvent<TArgs>, new() 
    where TArgs : Args => Publish<T, TArgs>(args, false);
  public Subscription Count<T, TArgs>()
      where T : TEvent<TArgs>, new()
      where TArgs : Args {
  if (typeof(T) == typeof(Count)) throw new InvalidOperationException("Cannot count Count events");
  else return Subscribe<T, TArgs>(args => Count(new($"❮{typeof(T).Name}❯")));
  }
  public Subscription Count<T>()
    where T : TEvent<NArgs>, new() => Count<T, NArgs>();
}