using System.Collections.Generic;
using UnityEngine;

public class HookSpawner : MonoBehaviour
{
    //Parameters
    public int nHooks = 3;
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
            GameObject hookObj = GetClosestHook(1.0f);
            isFiring = true;

            if (hookObj)
            {
                Destroy(hookObj);
                nHooks++;
            }
            else
            {
                if (nHooks > 0)
                {
                    Hook hookScr = Instantiate(hook, gunObj.transform.position, Quaternion.identity).GetComponent<Hook>();
                    hookScr.isStatic = false;
                    nHooks--;

                    lastFireTime = Time.time;
                }
            }
        }
        else if (isFiring)
        {
            if (!pInput.IsFiring())
                isFiring = false;
        }
    }

    GameObject GetClosestHook(float maxDistance)
    {
        KeyValuePair<GameObject, float> closest = new KeyValuePair<GameObject, float>(null, maxDistance);

        foreach (GameObject hookObj in GameObject.FindGameObjectsWithTag("Hook"))
        {
            Hook hook = hookObj.GetComponent<Hook>();

            if (!hook.isStatic)
            {
                float distance = Vector3.Distance(transform.position, hookObj.transform.position);

                if (distance < closest.Value)
                    closest = new KeyValuePair<GameObject, float>(hookObj, distance);
            }
        }

        return closest.Key;
    }
}
