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

  public bool beenCounting = false;
  public Boolean tagIsVisible(tag _tag) {
    if (!registeredTags.ContainsKey(_tag)) return false;
    if (_tag == tag.count && !beenCounting) {
      startCounting();
    }
    return registeredTags[_tag];
  }

  private void startCounting() {
    beenCounting = true;
    buttonCreator.rebuild();
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
  public void toggleTag(string tagName) {
    if (tagNameExists(tagName)) {
      var thag = resolveTag(tagName);
      registeredTags[thag] = !registeredTags[thag];
    } else {
      throw new Exception("Tag not found " + tagName);
    }
    buffer.publish();
  }
  //make this without creating
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
  public bool tagNameExists(string tagName) {
    var reversedTagHolder = registeredTags.Keys.ToDictionary(tag => tag.name, tag => tag);
    return reversedTagHolder.ContainsKey(tagName);
  }
  private static string getRandomColor() => $"{new Random().Next(0x1000000):X6}";
}