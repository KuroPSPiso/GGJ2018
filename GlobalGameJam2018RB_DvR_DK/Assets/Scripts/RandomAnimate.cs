using UnityEngine;

public class RandomAnimate : MonoBehaviour
{
    //Parameters
    public float moveSpeed = 1.0f;
    public float amplitude = 0.1f;

    //Private members
    float time = 0.0f;

    void Start()
    {
        time = Random.Range(0.0f, 1.0f);
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.z += Mathf.Sin(time * 2 * Mathf.PI) * amplitude;
        transform.position = pos;

        time += Time.deltaTime * moveSpeed;
        if (time > 1.0f)
            time -= 1.0f;
    }
}
