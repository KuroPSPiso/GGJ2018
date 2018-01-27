using UnityEngine;

public class HookSpawner : MonoBehaviour
{
    //Parameters
    public float cooldownTime = 1.0f;
    public Transform gunObj;
    public GameObject hook;

    //Private members
    float lastFireTime = 0.0f;
    bool isFiring = false;

    PlayerInput pInput;

    void Start()
    {
        pInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!pInput.IsActive())
            return;

        if (!isFiring && pInput.IsFiring())
        {
            if (Time.time - lastFireTime > cooldownTime)
            {
                Instantiate(hook, gunObj.transform.position, Quaternion.identity);

                lastFireTime = Time.time;
                isFiring = true;
            }
        }
        else if (isFiring)
        {
            if (!pInput.IsFiring())
                isFiring = false;
        }
    }
}
