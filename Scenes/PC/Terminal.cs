using Godot;
using System;
using System.Threading.Tasks;
using OBus;
[GlobalClass]
public partial class Terminal : TextEdit {
	public Bus bus;
	public GlobalState global;
	public override void _Ready()	{
		bus = GetNode<Bus>("/root/bus");
		global = GetNode<GlobalState>("/root/Global");
	}
	public async void AddText(string text) {
		while (text.Length > 0) {
			this.Text += text[0];
			text.Remove(0, 1);
			await Task.Delay(50);
		}
	}
}
