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

    public void Enable()
    {
        GetComponent<LineRenderer>().material = matEnabled;
    }
}
