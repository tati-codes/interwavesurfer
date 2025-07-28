using Godot;
using System;
using OBus;
public partial class AudioRoot : Node {
	public Bus bus;
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
	}

}

