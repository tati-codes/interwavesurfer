// meta-name: node w bus + global state acess
// meta-description: node with autoload bus and global state
// meta-default: true
// meta-space-indent: 2
using Godot;
using System;
using OBus;
public partial class _CLASS_ : _BASE_ {
  public Bus bus;
  public GlobalState global;
  public override void _Ready()	{
    bus = GetNode<Bus>("/root/bus");
    global = GetNode<GlobalState>("/root/Global");
  }
}