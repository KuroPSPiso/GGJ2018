using System;
using UnityEngine;

public class PackageTransfer : MonoBehaviour
{
    //Parameters
    public float maxDistance = 2.0f;

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
        if (pInput.IsActing())
        {
            GameObject packageObj = GetClosestPackage(maxDistance);

            if(packageObj)
            {

            }
        }
    }

    GameObject GetClosestPackage(float maxDistance = 2.0f)
    {
        Tuple<GameObject, float> closest = Tuple.Create<GameObject, float>(null, maxDistance);

        foreach (GameObject packageObj in GameObject.FindGameObjectsWithTag("Package"))
        {
            float distance = Vector3.Distance(transform.position, packageObj.transform.position);

            if (distance < closest.Item2)
                closest = Tuple.Create(packageObj, distance);
        }

        return closest.Item1;
    }
}
