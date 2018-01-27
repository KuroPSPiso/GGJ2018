using UnityEngine;

public class RopeColor : MonoBehaviour
{
    //Parameters
    public Material matEnabled;
    public Material matDisabled;
    public Material matCharged;

    public void Disable()
    {
        GetComponent<LineRenderer>().material = matDisabled;
    }

    public void ResetColor()
    {
        GetComponent<LineRenderer>().material = matEnabled;
    }

    public void Highlight()
    {
        GetComponent<LineRenderer>().material = matCharged;
    }
}
