using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Taterminal
{
  public struct tag
  {
      public string name { get; set; }
      public string bbcolor { get; set; } 
      public static tag nullTag = new tag { name = "NULL", bbcolor = "BLACK"};
      public static tag defaultTag = new tag { name = "INFO", bbcolor = "WHITE"};
      public static tag info = new tag()
      {
          name = "info",
          bbcolor = "WHITE",
      };
      public static tag warning = new tag()
      {
          name = "warn",
          bbcolor = "YELLOW",

      };
      public static tag error = new tag()
      {
          name = "error",
          bbcolor = "ff7f71",

      };
      public static tag evnt = new tag()
      {
          name = "event",
          bbcolor = "6e7177",
      };
      public static tag count = new tag()
      {
          name = "count",
          bbcolor = "BLUE",
      };
      public tag (string aname, string acolor)
      {
          name = aname;
          bbcolor = acolor;
      }
      public static bool isNullTag(tag thag)
      {
          return thag.name == "NULL" && thag.bbcolor == "BLACK";
      }
      public string envelop(string filling) {
        return this.parens().opn + filling + this.parens().cls;
      }
      public (string opn, string cls) parens(){
          if (bbcolor == "WHITE") return ("", "");
          if (bbcolor == "6e7177" && name == "event") return ($"[code][color={bbcolor}]", "[/color][/code]");
          if (bbcolor == "BLUE" && name == "count") return ($"[fill][bgcolor={bbcolor}]", " [/bgcolor][/fill]");
          return ($"[color={bbcolor}]", "[/color]");
      }
      public bool Equals(tag p)
      {
          return (name == p.name) && (bbcolor == p.bbcolor);
      }
      public override bool Equals(object obj) {
          return obj is tag other && Equals(other);
      }
      public static bool operator ==(tag t1, tag t2) => t1.Equals(t2);
      public static bool operator !=(tag t1, tag t2) => !t1.Equals(t2);
      public override int GetHashCode()
      {
        return HashCode.Combine(name, bbcolor);
      }
  }
  public class TagToggled : TEvent<NArgs> {}
}
