using UnityEngine;
using XInputDotNetPure;

public class ControllersManager : MonoBehaviour
{
	private ControllerControl[] _controllers;

	private void Start()
	{
		_controllers = new ControllerControl[4];
		GetControllers();
	}

	private void Update()
	{
		GetControllers();
		UpdateControllers();
	}

	private void GetControllers()
	{
		for (int i = 0; i < _controllers.Length; i++)
		{
			if (_controllers[i] == null && GamePad.GetState((PlayerIndex)i).IsConnected)
			{
				Debug.Log(string.Format("Player {0}  Connected", i));

				_controllers[i] = new ControllerControl
				{
					ControllerIndex = (PlayerIndex)i,
				};
			}
		}
	}

	private void UpdateControllers()
	{
		for (int i = 0; i < _controllers.Length; i++)
		{
			if (_controllers[i] != null)
			{
				_controllers[i].Update();
			}
		}
	}
}
