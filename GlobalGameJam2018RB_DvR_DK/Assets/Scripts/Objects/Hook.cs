using UnityEngine;

public class Hook : MonoBehaviour
{
    //Parameters
    public bool isStatic = true;
    [HideInInspector] public int isConnected = 0;

    public Material matSelect;
    public Material matDeselect;

    public void Select()
    {
        GetComponent<MeshRenderer>().material = matSelect;
    }

    public void Deselect()
    {
        GetComponent<MeshRenderer>().material = matDeselect;
    }
}
