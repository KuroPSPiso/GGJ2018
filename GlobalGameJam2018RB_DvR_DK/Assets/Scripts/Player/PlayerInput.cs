using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] ControllersManager controllersManager;
    [SerializeField] int controllerId;

    public Vector2 GetMovement()
    {
        return controllersManager.GetLeftAnalog(controllerId);
    }

    public Vector2 GetAiming()
    {
        return controllersManager.GetRightAnalog(controllerId);
    }

    public bool IsJumping()
    {
        return controllersManager.GetLeftTrigger(controllerId) > 0.25f;
    }

    public bool IsFiring()
    {
        return controllersManager.GetRightTrigger(controllerId) > 0.25f;
    }

    public bool IsActing()
    {
        return controllersManager.GetButton0(controllerId);
    }
}
