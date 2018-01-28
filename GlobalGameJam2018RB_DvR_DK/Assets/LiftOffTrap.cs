using UnityEngine;

public class LiftOffTrap : MonoBehaviour
{
    public int team;
    float time = 0.5f;

    void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0.0f)
            Destroy(gameObject);
    }
}
