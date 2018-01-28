using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCamera : MonoBehaviour
{
    [HideInInspector] public List<Vector3> cameraPos = new List<Vector3>();
    public float moveSpeed = 6.0f;
    int focusIndex = 0;
    float waitTime = 0.0f;
    public int winningTeam;

    public List<Text> labels = new List<Text>();

    void Update()
    {
        if (cameraPos.Count > 0)
        {
            if (waitTime > 0.0f)
                waitTime -= Time.deltaTime;
            else
            {
                float distance = Vector3.Distance(transform.position, cameraPos[focusIndex]);

                if (distance < 0.05f)
                {
                    foreach (Text label in labels)
                        label.gameObject.SetActive(true);

                    waitTime = 3.0f;
                    focusIndex = (focusIndex + 1) % 2;
                }
                else
                {
                    Vector3 moveDir = (cameraPos[focusIndex] - transform.position).normalized;
                    transform.position += moveDir * Mathf.Min(moveSpeed * Time.deltaTime, distance);
                }
            }
        }
    }

    public void SetWinLabel(int team)
    {
        foreach (Text label in labels)
            label.text = (team == 0 ? "Green" : "Blue") + " wins";
    }
}
