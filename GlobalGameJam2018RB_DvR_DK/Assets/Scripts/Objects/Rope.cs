using UnityEngine;

public class Rope : MonoBehaviour
{
    //Parameters
    public Material matEnabled;
    public Material matDisabled;

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
        GetComponent<LineRenderer>().material = matDisabled;
    }
}
