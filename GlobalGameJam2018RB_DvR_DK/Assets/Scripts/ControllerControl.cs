using XInputDotNetPure;

public class ControllerControl
{
	public PlayerIndex ControllerIndex { get; set; }
	public GamePadState PreviousState { get; set; }
	public GamePadState State { get; set; }

	public void Update()
	{
		PreviousState = State;
		State = GamePad.GetState(ControllerIndex);
	}
}