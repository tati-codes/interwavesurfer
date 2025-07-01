using Godot;
using System;
using Taterminal;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
[Tool][GlobalClass]
public partial class Buffer : Node
{
    [Export]
    public TagHolder tagHolder;
    [Export]
    public Output label;
    public bool collapsed = false; 
    LOG_LEVEL overrideLogLevel { get; set; } = (LOG_LEVEL)1;
    int msgCount = 0;
    string filterStr = String.Empty;
    Dictionary<terminalString, List<int>> rawBuffer = new();
    public Dictionary<terminalString, List<int>> countBuffer =>  rawBuffer.Where(kvp => kvp.Key.tag.name == "count").ToDictionary();
    public bool counting => countBuffer.Count > 0 && tagHolder.tagIsVisible(tag.count);
    Dictionary<terminalString, List<int>> workingBuffer =>
        filterStr != String.Empty ? rawBuffer.Where(kvp => kvp.Key.contains(filterStr)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value) : rawBuffer;
    Dictionary<terminalString, List<int>> filteredBuffer => workingBuffer.Where(kvp => tagHolder.tagIsVisible(kvp.Key.tag) && kvp.Key.tag.name != "count").ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    public string counts => tag.count.envelop(String.Join("[color=BLUE]:[/color]|[color=BLUE]:[/color]", countBuffer.Select(cstr => cstr.Key.details + ":[color=BLUE]:[/color]" + cstr.Value.Count)) + " ");
    List<(int count, terminalString str, int ocurrences)> collapsedBuffer => filteredBuffer.Select(kvp => (place: kvp.Value.Max(), str: kvp.Key, ocurrences: kvp.Value.Count)).OrderBy(x => x.place).ToList();
    List<terminalString> uncollapsedBuffer {
      get {
        terminalString[] strings = new terminalString[msgCount];
        foreach (var (str, ints) in filteredBuffer) {
          ints.ForEach(i => strings[i - 1] = str);
        }
        return strings.Where(s => !s.Equals(default)).ToList();
      }
    }
    public override void _Ready() {
        // bus.Subscribe<Log, Text>(consume);
        // bus.Subscribe<Warn, Text>(consume);
        // bus.Subscribe<Taterminal.Error, Text>(consume);
        // bus.Subscribe<BgLog, DbgString>(consume);
        // bus.Subscribe<TLog, DbgString>(consume);
        // bus.Subscribe<ClearTerminal>((args) => Clear());
        // bus.Subscribe<FilterStringChanged, Text>(setFilter);
        // bus.Subscribe<ToggleGlobalCollapse>(args => collapsed = !collapsed);
        // bus.Subscribe<Count, Text>(args => consume(Count.GetTerminalString(args)));
        // bus.Subscribe<TagToggled>(args => publish());
        consume(new Text("hi"));
    }

    public void toggleGlobalCollapse() { 
      collapsed = !collapsed;
      publish();
    }
    public void consume(terminalString msg) => Add(msg);
    public void consume(Text msg) => consume(new terminalString(msg, tagHolder.resolveTag(msg.tag)));
    public void consume(DbgString msg) => consume(new terminalString(msg,  tagHolder.resolveTag(msg.tag)));
    public void publish() => label.Display(this);
    public void Add(terminalString tStr) {
      msgCount++;
      if (rawBuffer.ContainsKey(tStr)) rawBuffer[tStr].Add(msgCount);
      else rawBuffer.Add(tStr, new() { msgCount });
      publish();
    }
    public void Clear() {
      msgCount = 0;
      rawBuffer.Clear();
      publish();
    }
    public void setFilter(string newFilterStr) {
      string oldFilter = filterStr;
      string newFilter = newFilterStr;
      if (oldFilter == newFilter) return;
      if (newFilterStr.Length == 0) {
        filterStr = string.Empty;
      } else {
        filterStr = newFilterStr;
      }
      publish();
    }
    public void setOverride(LOG_LEVEL newLog) {
      if (newLog == overrideLogLevel) return;
      overrideLogLevel = newLog;
      publish();
    }
    private string build()  { 
      if (collapsed) return buildCollapsed();
      var result = "";
      foreach (var str in uncollapsedBuffer) {
        var current = str.ToString(overrideLogLevel);
        if (current == "") continue;
        result += (current + "\n");
      }
      return result;
    }
    private string buildCollapsed() {
      string result = "";
      foreach (var (_, str, count) in collapsedBuffer) {
        var current = str.ToString(overrideLogLevel);
        if (current == "") continue;
        if (collapsed && count > 1) current += str.tag.envelop($" [u]x{count}[/u]");
        current += "\n";
        result += current;
      }
      return result;
    }
    public static implicit operator string(Buffer buffer) => buffer.build() + (buffer.counting ? buffer.counts : "");
}