using UnityEngine;

public class Terminal : MonoBehaviour
{
    void Update()
    {

    }

    public void StartLoading(int team)
    {
        Debug.Log("start loading " + team);
    }

    public void StopLoading(int team)
    {
        Debug.Log("stop loading " + team);
    }

    public void FinishLoading(int team)
    {
        Debug.Log("finish loading " + team);
    }
}
