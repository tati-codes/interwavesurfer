using System.Collections.Generic;
using Godot;

public static class PlayerInput {
    public static readonly string PRIMARY = "primary";
    public static readonly string LT = "left_trigger";
    public static readonly string RT = "right_trigger";
    public static readonly string RIGHT = "move_right";
    public static readonly string LEFT = "move_left";
    public static readonly string BACK = "move_backwards";
    public static readonly string FORWARD = "move_forwards";
    public static readonly string DEBUG = "debug";
    public static Dictionary<Actions, string> ActionsDict = new() {
      [Actions.RT] = RT,
      [Actions.LT] = LT,
      [Actions.PRIMARY] = PRIMARY,
      [Actions.RIGHT] = RIGHT,
      [Actions.LEFT] = LEFT,
      [Actions.BACK] = BACK,
      [Actions.FORWARD] = FORWARD,
      [Actions.DEBUG] = DEBUG,
    };
    public enum Actions {
      PRIMARY,
      LT,
      RT,
      RIGHT,
      LEFT,
      BACK,
      FORWARD,
      DEBUG,
      UNKNOWN
    }
    public enum Buttons {
      A,
      B,
      X,
      Y,
      LB,
      RB,
      TRIANGLE,
      DPAD_UP,
      DPAD_DOWN,
      DPAD_LEFT,
      DPAD_RIGHT,
      JOY_UP,
      JOY_DOWN,
      JOY_LEFT,
      JOY_RIGHT,
    }
    public static Actions EventIsAction(InputEvent @event) {
      if (@event.IsActionReleased(PRIMARY)) return Actions.PRIMARY;
      else if (@event.IsActionReleased(LT)) return Actions.LT;
      else if (@event.IsActionReleased(RT)) return Actions.RT;
      else if (@event.IsActionReleased(RIGHT)) return Actions.RIGHT;
      else if (@event.IsActionReleased(LEFT)) return Actions.LEFT;
      else if (@event.IsActionReleased(BACK)) return Actions.BACK;
      else if (@event.IsActionReleased(FORWARD)) return Actions.FORWARD;
      else if (@event.IsActionReleased(DEBUG)) return Actions.DEBUG;
      else return Actions.UNKNOWN;
    }
  }
