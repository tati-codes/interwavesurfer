// meta-name: node w bus
// meta-description: node with autoload bus
// meta-default: true
// meta-space-indent: 2
using Godot;
using System;
using Taterminal;
public partial class _CLASS_ : _BASE_ {
  public Bus bus;
	public override void _Ready()	{
    Bus bus = GetNode<Bus>("/root/bus");
	}

}
