using UnityEngine;

public class RopeColor : MonoBehaviour
{
    //Parameters
    public Material matEnabled;
    public Material matDisabled;
    public Material matCharged;
    public bool inTransfer = false;

    public void Disable()
    {
        GetComponent<LineRenderer>().material = matDisabled;
    }

    public void ResetColor()
    {
        if (!inTransfer)
            GetComponent<LineRenderer>().material = matEnabled;
    }

    public void Highlight()
    {
        GetComponent<LineRenderer>().material = matCharged;
    }

    public void Transfer()
    {
        Highlight();
        inTransfer = true;
    }

    public void ResetTransfer()
    {
        inTransfer = false;
        ResetColor();
    }
}
