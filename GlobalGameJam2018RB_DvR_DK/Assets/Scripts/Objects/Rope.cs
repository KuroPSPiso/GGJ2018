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
        Debug.Log("reset");
    }

    public void Highlight()
    {
        GetComponent<LineRenderer>().material = matDisabled;
        Debug.Log("highlight");
    }
}
