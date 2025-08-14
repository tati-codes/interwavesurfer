#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotInk;
using Ink.Runtime;
using InkBridge;
using Interactables;
using OBus;
using QuizSpace;
using State;
using TatiDebug;
using UIEvents;

public partial class GlobalState : Node {
  public Node CurrentScene { get; set; }
	public sceneEnum currentScene = sceneEnum.PC;
  public Dictionary<sceneEnum, string> scenes = new Dictionary<sceneEnum, string>() {
    [sceneEnum.ISLAND] = "res://Scenes/Boiat/Outside.tscn",
    [sceneEnum.QUIZ] = "res://Scenes/QuizSpace/Quiz.tscn",
    [sceneEnum.PC] = "res://Scenes/PC/pc_root.tscn",
    [sceneEnum.MAIN_MENU] = "res://Scenes/Menu/MainMenu.tscn",
  };
  void sceneSwitch(SceneArgs args) {
    Viewport root = GetTree().Root;
    string scene =  scenes[args.scene];
    CallDeferred(MethodName.DeferredGotoScene, scene);
  }
  void DeferredGotoScene(string path) {
    CurrentScene.Free();
    var nextScene = GD.Load<PackedScene>(path);
    CurrentScene = nextScene.Instantiate();
    GetTree().Root.AddChild(CurrentScene);
    GetTree().CurrentScene = CurrentScene;
  }
}

namespace State {
  public enum sceneEnum {
    MAIN_MENU,
    QUIZ,
    PC,
    ISLAND,
    HOUSE
  }
  public class GoToScene : TEvent<SceneArgs> { }
  public class SceneArgs(sceneEnum scene) : Args {
    public sceneEnum scene { get; init; } = scene;
  }
}