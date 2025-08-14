using Godot;
using System;
using System.Collections.Generic;
using Godot.NativeInterop;
using OBus;
public partial class InsideRoot : Node3D {
	public Bus bus;
	public GlobalState global;
	private SubscriptionHolder subscriptions = new();

	public enum zone {
		LIVING,
		BATH, 
		TOWER
	}
	
	[Export]
	public Camera3D LivingRoomCam {get; set;} 
	[Export]
	public Camera3D BathCam {get; set;} 
	[Export]
	public Camera3D TowerCam {get; set;} 
	 [Export]
	 public Area3D Towerzone {get; set;}
	 [Export]
	 public Area3D Bathzone {get; set;}
	 [Export]
	 public Area3D Livingzone {get; set;}
	 [Export]
	 public Player player {get; set;} 
	  
  

	 private Dictionary<zone, Camera3D> cameras = new();
	 zone currentZone = zone.LIVING;
	public override void _ExitTree() {
		subscriptions.Dispose();
		base._ExitTree();
	}
	public override void _Ready()	{
		cameras.Add(zone.LIVING, LivingRoomCam);
		cameras.Add(zone.BATH, BathCam);
		cameras.Add(zone.TOWER, TowerCam);
		bus = GetNode<Bus>("/root/bus");
		global = GetNode<GlobalState>("/root/Global");
		Livingzone.BodyEntered += (args) => setZone(zone.LIVING);
		Bathzone.BodyEntered += (args) => setZone(zone.BATH);
		Towerzone.BodyEntered += (args) => setZone(zone.TOWER);
	}

	void setZone(zone zone) {
		if (zone == currentZone) return;
		currentZone = zone;
		cameras[currentZone].MakeCurrent();
		// player.cam =  cameras[currentZone];
	}
}
