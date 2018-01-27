using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ControllersManager : MonoBehaviour
{
    public enum ControllerMappingType
    {
        Plugin,
        Both
    }

    private const int MaxPlayerCount = 4;
    private ControllerControl[] _controllers;
    private Dictionary<int, bool> _playerReadyState = new Dictionary<int, bool>();

    public ControllerMappingType MappingType = ControllerMappingType.Plugin;
    private Dictionary<int, bool> _genericButtonDictionary = new Dictionary<int, bool>(9);

    public int PlayerCount
    {
        get
        {
            int playerCount = 0;

            for (int i = 0; i < MaxPlayerCount; i++)
            {
                if (_playerReadyState.ContainsKey(i))
                {
                    bool activePlayer;
                    _playerReadyState.TryGetValue(i, out activePlayer);

                    if (activePlayer)
                    {
                        playerCount++;
                    }
                }
            }

            return playerCount;
        }
    }
    public Dictionary<int, bool> PlayerReadyState
    {
        get
        {
            return _playerReadyState;
        }
    }

    private void Start()
	{
	    this.Reset();
	    _controllers = new ControllerControl[4];
	    GetControllers();
    }

	private void Update()
	{
		GetControllers();
		UpdateControllers();
	    UpdateReadyState();
    }

    private void UpdateReadyState()
    {
        for (int i = 0; i < MaxPlayerCount; i++)
        {
            if (this.IsControllerActive(i))
            {
                if (this.GetButtonStart(i, true))
                {
                    if (this._playerReadyState.ContainsKey(i))
                    {
                        bool value = false;
                        if (this._playerReadyState.TryGetValue(i, out value))
                        {
                            this._playerReadyState[i] = !value;
                            Debug.Log(string.Format("Player /w Controller {0} is ready? {1}.", i, this._playerReadyState[i]));
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Reset the active players
    /// </summary>
    public void Reset()
    {
        //Reset Player state
        for (int i = 0; i < MaxPlayerCount; i++)
        {
            this._playerReadyState.Add(i, false);
        }

        //Reset Generic Button
        for (int i = 0; i < 9; i++)
        {
            this._genericButtonDictionary.Add(i, false);
        }
    }

    private void GetControllers()
	{
	    for (int i = 0; i < _controllers.Length; i++)
	    {
	        if (_controllers[i] == null && GamePad.GetState((PlayerIndex)i).IsConnected)
	        {
	            _controllers[i] = new ControllerControl
	            {
	                ControllerIndex = (PlayerIndex)i
	            };
	        }
	    }
    }

    private int GetIndexOfGenericController()
    {
        string[] joystickNames = Input.GetJoystickNames();

        for (int i = 0; i < joystickNames.Length; i++)
        {
            if (joystickNames[i].ToLower().Contains("generic"))
            {
                return i;
            }
        }

        return -1;
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

    public Vector2 GetLeftAnalog(int controllerId)
    {
        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            return new Vector2(
                    Input.GetAxis(string.Format("Joy{0}LeftX", GetIndexOfGenericController() + 1)),
                    Input.GetAxis(string.Format("Joy{0}LeftY", GetIndexOfGenericController() + 1))
                );
        }

        return new Vector2(
            _controllers[controllerId].State.ThumbSticks.Left.X,
            _controllers[controllerId].State.ThumbSticks.Left.Y
            );
    }

    public Vector2 GetRightAnalog(int controllerId)
    {
        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            return new Vector2(
                Input.GetAxis(string.Format("Joy{0}RightX", GetIndexOfGenericController() + 1)),
                Input.GetAxis(string.Format("Joy{0}RightY", GetIndexOfGenericController() + 1))
            );
        }

        return new Vector2(
            _controllers[controllerId].State.ThumbSticks.Right.X,
            _controllers[controllerId].State.ThumbSticks.Right.Y
            );
    }

    public Vector2 GetHat(int controllerId, bool mustRelease = false)
    {

        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            return new Vector2();
        }

        float up = 0;
        float down = 0;
        float left = 0;
        float right = 0;

        if (!mustRelease)
        {
            up = (_controllers[controllerId].State.DPad.Up == ButtonState.Pressed)
                ? 1 : 0;
            down = (_controllers[controllerId].State.DPad.Down == ButtonState.Pressed)
                ? 1 : 0;
            left = (_controllers[controllerId].State.DPad.Left == ButtonState.Pressed)
                ? 1 : 0;
            right = (_controllers[controllerId].State.DPad.Right == ButtonState.Pressed)
                ? 1 : 0;
        }
        else
        {
            up = (_controllers[controllerId].PreviousState.DPad.Up == ButtonState.Released &&
                  _controllers[controllerId].State.DPad.Up == ButtonState.Pressed)
                ? 1 : 0;
            down = (_controllers[controllerId].PreviousState.DPad.Down == ButtonState.Released &&
                  _controllers[controllerId].State.DPad.Down == ButtonState.Pressed)
                ? 1 : 0;
            left = (_controllers[controllerId].PreviousState.DPad.Left == ButtonState.Released &&
                  _controllers[controllerId].State.DPad.Left == ButtonState.Pressed)
                ? 1 : 0;
            right = (_controllers[controllerId].PreviousState.DPad.Right == ButtonState.Released &&
                  _controllers[controllerId].State.DPad.Right == ButtonState.Pressed)
                ? 1 : 0;
        }

        return new Vector2(
            0 - left + right,
            0 - down + up
            );
    }

    public float GetLeftTrigger(int controllerId)
    {
        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            return (Input.GetButton(string.Format("Joy{0}Button6", GetIndexOfGenericController() + 1)) ? 0f : 1f);
        }

        return _controllers[controllerId].State.Triggers.Left;
    }

    public float GetRightTrigger(int controllerId)
    {
        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            return (Input.GetButton(string.Format("Joy{0}Button7", GetIndexOfGenericController() + 1)) ? 0f : 1f);
        }

        return _controllers[controllerId].State.Triggers.Right;
    }

    /// <summary>
    /// Get B (XBox), Get Circle (Playstation)
    /// </summary>
    public bool GetButton0(int controllerId, bool mustRelease = false)
    {

        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            bool newState = Input.GetButton(string.Format("Joy{0}Button1", GetIndexOfGenericController() + 1));
            bool oldState = this._genericButtonDictionary[1];

            if (!mustRelease)
            {
                this._genericButtonDictionary[1] = newState;
                return newState;
            }

            if (oldState == false && newState == true)
            {
                this._genericButtonDictionary[1] = newState;
                return true;
            }

            this._genericButtonDictionary[1] = newState;
            return false;
        }

        if (!mustRelease)
        {
            return _controllers[controllerId].State.Buttons.B == ButtonState.Pressed;
        }

        return _controllers[controllerId].PreviousState.Buttons.B == ButtonState.Released &&
               _controllers[controllerId].State.Buttons.B == ButtonState.Pressed;
    }
    /// <summary>
    /// Get A (XBox), Get Cross (Playstation)
    /// </summary>
    public bool GetButton1(int controllerId, bool mustRelease = false)
    {

        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            bool newState = Input.GetButton(string.Format("Joy{0}Button2", GetIndexOfGenericController() + 1));
            bool oldState = this._genericButtonDictionary[2];

            if (!mustRelease)
            {
                this._genericButtonDictionary[2] = newState;
                return newState;
            }

            if (oldState == false && newState == true)
            {
                this._genericButtonDictionary[2] = newState;
                return true;
            }

            this._genericButtonDictionary[2] = newState;
            return false;
        }

        if (!mustRelease)
        {
            return _controllers[controllerId].State.Buttons.A == ButtonState.Pressed;
        }

        return _controllers[controllerId].PreviousState.Buttons.A == ButtonState.Released &&
               _controllers[controllerId].State.Buttons.A == ButtonState.Pressed;
    }
    /// <summary>
    /// Get X (XBox), Get Square (Playstation)
    /// </summary>
    public bool GetButton2(int controllerId, bool mustRelease = false)
    {

        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            bool newState = Input.GetButton(string.Format("Joy{0}Button3", GetIndexOfGenericController() + 1));
            bool oldState = this._genericButtonDictionary[3];

            if (!mustRelease)
            {
                this._genericButtonDictionary[3] = newState;
                return newState;
            }

            if (oldState == false && newState == true)
            {
                this._genericButtonDictionary[3] = newState;
                return true;
            }

            this._genericButtonDictionary[3] = newState;
            return false;
        }

        if (!mustRelease)
        {
            return _controllers[controllerId].State.Buttons.X == ButtonState.Pressed;
        }

        return _controllers[controllerId].PreviousState.Buttons.X == ButtonState.Released &&
               _controllers[controllerId].State.Buttons.X == ButtonState.Pressed;
    }
    /// <summary>
    /// Get Y (XBox), Get Triangle (Playstation)
    /// </summary>
    public bool GetButton3(int controllerId, bool mustRelease = false)
    {

        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            bool newState = Input.GetButton(string.Format("Joy{0}Button0", GetIndexOfGenericController() + 1));
            bool oldState = this._genericButtonDictionary[0];

            if (!mustRelease)
            {
                this._genericButtonDictionary[0] = newState;
                return newState;
            }

            if (oldState == false && newState == true)
            {
                this._genericButtonDictionary[0] = newState;
                return true;
            }

            this._genericButtonDictionary[0] = newState;
            return false;
        }

        if (!mustRelease)
        {
            return _controllers[controllerId].State.Buttons.Y == ButtonState.Pressed;
        }

        return _controllers[controllerId].PreviousState.Buttons.Y == ButtonState.Released &&
               _controllers[controllerId].State.Buttons.Y == ButtonState.Pressed;
    }

    public bool GetButtonLeft(int controllerId, bool mustRelease = false)
    {

        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            bool newState = Input.GetButton(string.Format("Joy{0}Button4", GetIndexOfGenericController() + 1));
            bool oldState = this._genericButtonDictionary[4];

            if (!mustRelease)
            {
                this._genericButtonDictionary[4] = newState;
                return newState;
            }

            if (oldState == false && newState == true)
            {
                this._genericButtonDictionary[4] = newState;
                return true;
            }

            this._genericButtonDictionary[4] = newState;
            return false;
        }

        if (!mustRelease)
        {
            return _controllers[controllerId].State.Buttons.LeftShoulder == ButtonState.Pressed;
        }

        return _controllers[controllerId].PreviousState.Buttons.LeftShoulder == ButtonState.Released &&
               _controllers[controllerId].State.Buttons.LeftShoulder == ButtonState.Pressed;
    }

    public bool GetButtonRight(int controllerId, bool mustRelease = false)
    {

        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            bool newState = Input.GetButton(string.Format("Joy{0}Button5", GetIndexOfGenericController() + 1));
            bool oldState = this._genericButtonDictionary[2];

            if (!mustRelease)
            {
                this._genericButtonDictionary[5] = newState;
                return newState;
            }

            if (oldState == false && newState == true)
            {
                this._genericButtonDictionary[5] = newState;
                return true;
            }

            this._genericButtonDictionary[5] = newState;
            return false;
        }

        if (!mustRelease)
        {
            return _controllers[controllerId].State.Buttons.RightShoulder == ButtonState.Pressed;
        }

        return _controllers[controllerId].PreviousState.Buttons.RightShoulder == ButtonState.Released &&
               _controllers[controllerId].State.Buttons.RightShoulder == ButtonState.Pressed;
    }

    public bool GetButtonStart(int controllerId, bool mustRelease = false)
    {


        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            bool newState = Input.GetButton(string.Format("Joy{0}Button9", GetIndexOfGenericController() + 1));
            bool oldState = this._genericButtonDictionary[2];

            if (!mustRelease)
            {
                this._genericButtonDictionary[9] = newState;
                return newState;
            }

            if (oldState == false && newState == true)
            {
                this._genericButtonDictionary[9] = newState;
                return true;
            }

            this._genericButtonDictionary[9] = newState;
            return false;
        }


        if (!mustRelease)
        {
            return _controllers[controllerId].State.Buttons.Start == ButtonState.Pressed;
        }

        return _controllers[controllerId].PreviousState.Buttons.Start == ButtonState.Released &&
               _controllers[controllerId].State.Buttons.Start == ButtonState.Pressed;
    }
    /// <summary>
    /// Get Back (XBox), Get Select (Playstation)
    /// </summary>
    public bool GetButtonAltStart(int controllerId, bool mustRelease = false)
    {

        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            bool newState = Input.GetButton(string.Format("Joy{0}Button8", GetIndexOfGenericController() + 1));
            bool oldState = this._genericButtonDictionary[8];

            if (!mustRelease)
            {
                this._genericButtonDictionary[8] = newState;
                return newState;
            }

            if (oldState == false && newState == true)
            {
                this._genericButtonDictionary[8] = newState;
                return true;
            }

            this._genericButtonDictionary[8] = newState;
            return false;
        }

        if (!mustRelease)
        {
            return _controllers[controllerId].State.Buttons.Back == ButtonState.Pressed;
        }

        return _controllers[controllerId].PreviousState.Buttons.Back == ButtonState.Released &&
               _controllers[controllerId].State.Buttons.Back == ButtonState.Pressed;
    }

    /// <summary>
    /// Get Guide (XBox), Get Select (Playstation)
    /// </summary>
    public bool GetButtonHome(int controllerId, bool mustRelease = false)
    {
        return false;

        if (!mustRelease)
        {
            return _controllers[controllerId].State.Buttons.Guide == ButtonState.Pressed;
        }

        return _controllers[controllerId].PreviousState.Buttons.Guide == ButtonState.Released &&
               _controllers[controllerId].State.Buttons.Guide == ButtonState.Pressed;
    }

    public bool IsControllerActive(int controllerId)
    {
        if (MappingType == ControllerMappingType.Both && controllerId == 3)
        {
            return GetIndexOfGenericController() != -1;
        }

        return _controllers[controllerId] != null;
    }
}