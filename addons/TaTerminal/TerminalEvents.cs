using Godot;
using System;
using System.Reflection;

namespace Taterminal {
  #region Logging
    public class Text : Args {
        public string text { get; set; }
        public Text(string t) {
            text = t;
            this.tag = Taterminal.tag.info.name;
        }
        public static Text error(string t) => new Text(t) { tag = Taterminal.tag.error.name};
        public static Text warn(string t) => new Text(t) { tag = Taterminal.tag.warning.name};
        public static Text evnt(string t) => new Text(t) { tag = Taterminal.tag.evnt.name};
        public static Text count(string t) => new Text(t) { tag = Taterminal.tag.count.name};
        public static implicit operator string(Text text) => text.text;
    }
    public class Log : TEvent<Text> { }
    public class TLog : TEvent<DbgString> { }
    public class Warn : TEvent<Text> { }
    public class Error : TEvent<Text> { }

    #region Counting
      public class Count : TEvent<Text> {
        public static terminalString GetTerminalString(Text label)  => new terminalString("count", label.text, tag.count, LOG_LEVEL.MINIMAL);
      }
      public class StopCount : TEvent<Text> {}
    #endregion

  #endregion
    
  #region Base Bus Classes
/// <summary>
/// How to do one-off subscriptions:
/// <para>Example:</para>
/// <para><c>Disposable subscription = null;</c></para>
/// <para><c>subscription = bus.Subscribe&lt;Count, Text&gt;(args =&gt; {</c></para>
/// <para><c> consume(Count.GetTerminalString(args));</c></para>
/// <para><c>  subscription?.Dispose();</c></para>
/// <para><c>});</c></para>
/// </summary>

    public class Subscription : IDisposable {
      private Action _unsubscribe;
      public Subscription(Action unsub) => _unsubscribe = unsub;
      public void Dispose() => _unsubscribe?.Invoke();
    }
    public enum LOG_LEVEL
    {
        SILENT,
        MINIMAL,
        DETAILED
    }
    public abstract class Args {
        public virtual LOG_LEVEL level { get; set; } = LOG_LEVEL.MINIMAL;
        public virtual string tag {get; set;} = Taterminal.tag.evnt.name;
        public string DebugStringify() {
            Type myClassType = this.GetType();
            PropertyInfo[] properties = myClassType.GetProperties();
            string result = "";
            
            foreach (PropertyInfo property in properties)
            {
                if (property.Name == "level" || property.Name == "tag") continue;
                result += property.Name + ": " + property.GetValue(this, null) + "\n";
            }
            
            return result;
        }
    }
    public abstract class TEvent<TArgs> where TArgs : Args {

        private Action<TArgs> _actions = args => { };
        // public void Subscribe(Action<TArgs> action) => _actions += action;
        public Subscription Subscribe(Action<TArgs> action) {
          _actions += action;
          return new Subscription(() => _actions -= action);
        }
        public void Publish(TArgs message) => _actions(message);
    }
    public class NArgs : Args { }
    public class DbgString : Args {

      private static bool isMessage<T>(T @event) {
        if (@event is Log || @event is Warn || @event is Taterminal.Error || @event is Count) return true;
        return false;
      }
      public static DbgString createFrom<T, TArgs>(T @event, TArgs args)
          where T : TEvent<TArgs>, new()
          where TArgs : Args 
      {
        if (isMessage<T>(@event) && @event is not Count && args is Text textargs) return createFromMessage(textargs);
        else if (isMessage<T>(@event) && @event is Count && args is Text countargs) return createFromCount(countargs);
        else return createFromEvent<T, TArgs>(args);
      }
      public static DbgString createFromMessage(Text textargs) => new DbgString() {
        name = "",
        text = textargs.text,
        level = textargs.level,
        tag = textargs.tag
      };
      public static DbgString createFromEvent<T, TArgs>(TArgs args)
        where T : TEvent<TArgs>, new()
        where TArgs : Args => new DbgString() {
        name = typeof(T).Name,
        text = args.DebugStringify(),
        level = args.level,
        tag = args.tag
      };
      public static DbgString createFromCount(Text textargs) => new DbgString() {
        name = "count",
        text = textargs.text,
        level = LOG_LEVEL.MINIMAL,
        tag = Taterminal.tag.count.name
      };
      public string name { get; set; }
      public string text { get; set; }
      public override string ToString() {
          return name + " " + text;
      }
      public string export() {
          var p1 = "{\n";
          var p2 = "\n}";
          var n = $"\"name\": \"{name}\",\n";
          var t = $"\"text\": \"{text}\",\n";
          var l = $"\"level\": \"{level.ToString()}\",\n";
          string tg = $"\"tag\": \"{tag}\"";
          return p1 + n + t + l + tg + p2;
      }
      public static DbgString import(string msg) {
          var pollo = Json.ParseString(msg).AsGodotDictionary();
          string name = pollo["name"].AsString();
          string text = pollo["text"].AsString();
          string level = pollo["level"].AsString();
          string tag = pollo["tag"].AsString();
          return new DbgString()
          {
              name = name,
              text = text,
              level = Enum.Parse<LOG_LEVEL>(level),
              tag = tag
          };
      }
    }
    public class BgLog : TEvent<DbgString> { }
  #endregion
    

}
