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
    CAN_MOVE,
    IS_HOLDING,
    DIALOG,
    FULLSCREEN_DIALOG,
    CHOICE_DIALOG,
  }
  /// <summary>
  /// Internal event for imperatively invoking the Dialog UI state.
  /// </summary>
  public class IShowDialog : TEvent<DialogText> {}
  public class FullscreenIShowDialog : TEvent<DialogText> {}
  public class DialogText(string content, string name, bool isLast = false) : Args {
    public string name { get; init; } = name;
    public string content { get; init; } = content;
    public bool isLast { get; init; } = isLast;
  }
}
