using Godot;
using System;
using InkBridge;
using OBus;
using QuizSpace;
using TatiDebug;

public partial class Boat : CharacterBody3D {
	[Export] public Node3D Foam { get; set; }

	[Export] public NavigationAgent3D navAgen { get; set; }
	[Export] public MeshInstance3D BoatMesh { get; set; }
	[Export]
	public AnimationPlayer anim {get; set;} 
	 
  
	public Vector3 initialPosition { get; set; }
	bool reached = true;
	//TODO on select look at choices;
	private Bus bus;

	public override void _Ready() {
		bus = GetNode<Bus>("/root/bus");
		bus.Subscribe<SailTowards, Location>(args => {
			navAgen.SetTargetPosition(args.coords);
			reached = false;
		});
		bus.Subscribe<QuizStateTransition, QuizStateTransitionArgs>(handleQuizStateTransition);
		navAgen.NavigationFinished += targetReached;
		initialPosition = this.Position;
		anim?.Play("Sway");
	}

	public override void _PhysicsProcess(double delta) {
		if (!reached) {
			var destination = navAgen.GetNextPathPosition();
			var local_dest = destination - GlobalPosition;
			var direction = local_dest.Normalized();
			Velocity = direction * 7;
			MoveAndSlide();
			if (Velocity.IsEqualApprox(Vector3.Zero)) {
				Foam.Hide();
				//TODO? incremental fade
			} else Foam.Show();
		}
	}

	void targetReached() {
		bus.Publish<BoatReachedPortal>();
		this.Hide();
		this.Position = initialPosition;
		reached = true;
	}

	void handleQuizStateTransition(QuizStateTransitionArgs args) {
		if (args.from == QuizState.SHOW_QUIZ_QUESTION && args.to == QuizState.AWAIT_PLAYER_CHOICE) {
			this.Show();
		}
	}
}

namespace QuizSpace {
  public class BoatReachedPortal : TEvent<NArgs> { }

	public class Location(Godot.Vector3 coords) : Args {
		public Godot.Vector3 coords { get; init; } = coords;
	}	
	public class SailTowards : TEvent<Location> { }
}	