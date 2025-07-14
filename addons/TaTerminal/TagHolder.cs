using Godot;
using System;
using System.Collections.Generic;
using Taterminal;
using System.Linq;

[Tool][GlobalClass]
public partial class TagHolder : Node {
  [Export]
  public Buffer buffer;
  [Export]
	public ButtonCreator buttonCreator { get; set; }

  public Dictionary<tag, bool> registeredTags = new Dictionary<tag, bool>() {
      [tag.info] = true,
      [tag.warning] = true,
      [tag.error] = true,
      [tag.evnt] = true,
      [tag.count] = true
  };
  public Boolean tagIsVisible(tag _tag) {
    if (registeredTags.ContainsKey(_tag)) {
      return registeredTags[_tag];
    } else {
      //TODO bus.Error("Tag not found", new Exception("Tag not found " + _tag.name));
      return false;
    }
  }
  public void registerTag(tag thag) {
    registeredTags.Add(thag, true);
    buttonCreator.rebuild();
  }
  public void toggleTag(tag thag) {
    if (registeredTags.ContainsKey(thag)) {
      registeredTags[thag] = !registeredTags[thag];
    } else {
      throw new Exception("Tag not found " + thag.name);
    }
    buffer.publish();
  }
  public tag resolveTag(string _tag) {
    var reversedTagHolder = registeredTags.Keys.ToDictionary(tag => tag.name, tag => tag);
    if (reversedTagHolder.ContainsKey(_tag)) {
      return reversedTagHolder[_tag];
    } else {
      tag theNewTag = new tag(_tag, getRandomColor());
      registerTag(theNewTag);
      return theNewTag;
    }
  }
  private static string getRandomColor() => $"{new Random().Next(0x1000000):X6}";
}