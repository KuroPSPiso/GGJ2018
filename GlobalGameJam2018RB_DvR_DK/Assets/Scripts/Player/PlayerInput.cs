﻿using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public ControllersManager controllersManager;
    public int controllerId;
	public Animator Animator;

    public RopeManager ropeManager;
    public int team;

    public bool disableInput = false;

    public bool IsActive()
    {
        if (disableInput)
            return false;

        return controllersManager.IsControllerActive(controllerId);
    }

    public Vector2 GetMovement()
    {
        return controllersManager.GetLeftAnalog(controllerId);
    }

    public Vector2 GetAiming()
    {
        return controllersManager.GetRightAnalog(controllerId);
    }

    public bool IsJumping(bool mustRelease = false)
    {
        return controllersManager.GetButton1(controllerId, mustRelease);
    }

    public bool IsTrapping(bool mustRelease = false)
    {
        return controllersManager.GetButton0(controllerId, mustRelease);
    }

    public bool IsFiring()
    {
        return controllersManager.GetRightTrigger(controllerId) > 0.25f;
    }

    public bool IsActing()
    {
        return controllersManager.GetLeftTrigger(controllerId) > 0.25f;
    }
}
