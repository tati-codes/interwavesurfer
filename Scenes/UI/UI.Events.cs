using OBus;

namespace UIEvents {
  public class UITransitionEv: TEvent<UITransition> { }
  public class UITransition(UIState from, UIState to) : Args {
    public UIState from{ get; init; } = from;
    public UIState to { get; init; } = to;
  }
  public enum UIState {
    IDLE,
    CAN_PICK_UP,
    CAN_READ, 
    //CAN_INTERACT
    IS_HOLDING,
    DIALOG,
  }
}
