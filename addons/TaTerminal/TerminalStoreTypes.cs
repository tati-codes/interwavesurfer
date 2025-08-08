using System;

namespace Taterminal {
  public struct terminalString {
      public string name { get; private set; }
      public string details  { get; private set; }
      public tag tag { get; private set; }
      public LOG_LEVEL logLevel { get; private set; }
      public terminalString(Text msg, tag _tag) {
        details = "";
        name = msg.text;
        tag = _tag;
        logLevel = msg.level;
      }
      public terminalString(DbgString msg, tag _tag) {
        name = msg.name;
        details = msg.text;
        tag = _tag;
        logLevel = msg.level;
        if (msg.name == "") { 
          details = "";
          name = msg.text;
        }
      }
      public terminalString(string _name, string _details, tag _tag, LOG_LEVEL _logLevel) {
        name = _name;
        details = _details;
        tag = _tag;
        logLevel = _logLevel;
      }
      public string ToString(LOG_LEVEL overrideLevel) {
        if (logLevel == overrideLevel) return this.auto();
        switch (overrideLevel)
        {
          case LOG_LEVEL.SILENT:
            if (logLevel == LOG_LEVEL.MINIMAL || logLevel == LOG_LEVEL.SILENT) {
              return this.manual(LOG_LEVEL.SILENT);
            } else return this.auto();
          case LOG_LEVEL.MINIMAL:
            return this.auto();
          case LOG_LEVEL.DETAILED:
            return this.manual(LOG_LEVEL.DETAILED);
          default:
            throw new Exception("no overrideLevel");
        }
      } 
      public bool contains(string searchStr) {
          return (this.name.ToLower().Contains(searchStr.ToLower()) || this.details.ToLower().Contains(searchStr.ToLower()));
      }
      private string auto() => manual(logLevel);
      private string manual(LOG_LEVEL _override) {
        var computed_details = details == "" ? "" : (": {\n" + "\t" + details + "}");
        switch (_override)
          {
              case LOG_LEVEL.SILENT:
                  if (this.tag.name == "info") return this.manual(LOG_LEVEL.MINIMAL);
                  return "";
              case LOG_LEVEL.MINIMAL:
                  return tag.envelop(name);
              case LOG_LEVEL.DETAILED:
                  return tag.envelop(name + computed_details);
              default:
                  throw new ArgumentException($"Unknown log level: {_override}");
          } 
      }
      public override bool Equals(object obj) {
          return obj is terminalString other && Equals(other);
      }
      public bool Equals(terminalString other) {
        return (this.name == other.name &&
                this.details == other.details &&
                this.tag == other.tag &&
                this.logLevel == other.logLevel);
      }
      public static bool operator ==(terminalString strA, terminalString strB) {
        return strA.Equals(strB);
      }
      public static bool operator !=(terminalString strA, terminalString strB) {
        return !strA.Equals(strB);
      }
      public override int GetHashCode() {
        return HashCode.Combine(name, details, tag, logLevel);
      }
  }
  public class ITag : Args
  {
      public new tag tag;
  }
  public class ToggleTag : TEvent<ITag> { }
  public class TerminalText : Args
  {
      public string text { get; set; }
  }
  public class TerminalTextUpdated : TEvent<Text>
  {

  }
  public class ClearTerminal : TEvent<NArgs> { }
}
