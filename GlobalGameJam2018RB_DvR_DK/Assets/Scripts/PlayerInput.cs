using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public int ControllerId;
    public ControllersManager controllerManager;

    public void Update()
    {
        if (controllerManager == null)
        {
            return;
        }
        if (!controllerManager.IsControllerActive(ControllerId))
        {
            return;
        }
        

        //Sticks
        Debug.Log(string.Format("L-A {0}", controllerManager.GetLeftAnalog(ControllerId)));
        Debug.Log(string.Format("R-A {0}", controllerManager.GetRightAnalog(ControllerId)));
        Debug.Log(string.Format("HAT {0}", controllerManager.GetHat(ControllerId)));
        //AXIS
        Debug.Log(string.Format("L-T {0}", controllerManager.GetLeftTrigger(ControllerId)));
        Debug.Log(string.Format("R-T {0}", controllerManager.GetRightTrigger(ControllerId)));
        //Buttons
        Debug.Log(string.Format("B-B {0}", controllerManager.GetButton0(ControllerId)));
        Debug.Log(string.Format("A-B {0}", controllerManager.GetButton1(ControllerId)));
        Debug.Log(string.Format("X-B {0}", controllerManager.GetButton2(ControllerId)));
        Debug.Log(string.Format("Y-B {0}", controllerManager.GetButton3(ControllerId)));
        Debug.Log(string.Format("L-S {0}", controllerManager.GetButtonLeft(ControllerId)));
        Debug.Log(string.Format("R-S {0}", controllerManager.GetButtonRight(ControllerId)));
        Debug.Log(string.Format("H-B {0}", controllerManager.GetButtonHome(ControllerId)));
        Debug.Log(string.Format("S-B {0}", controllerManager.GetButtonStart(ControllerId)));
        Debug.Log(string.Format("ASB {0}", controllerManager.GetButtonAltStart(ControllerId)));

    }
}
