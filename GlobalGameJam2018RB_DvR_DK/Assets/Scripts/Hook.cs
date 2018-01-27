using UnityEngine;

public class Hook : MonoBehaviour
{
    public void Select()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void Deselect()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
