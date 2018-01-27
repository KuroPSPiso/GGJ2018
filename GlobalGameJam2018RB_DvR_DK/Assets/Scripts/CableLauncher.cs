using System;
using System.Collections.Generic;
using UnityEngine;

public class CableLauncher : MonoBehaviour
{
    //Parameters
    public float maxDistance = 10f;
    public float maxAngle = 30f;
    public Transform gunObj;

    public ControllersManager controllersManager;
    public int controllerId;

    //Private members
    AngleCollection hooksCache;

    bool isFiring = false;
    Hook selectedHook;

    //Constants
    const string hookTag = "Hook";

    void Update()
    {
        //Get aiming direction
        Vector2 aimDirection = controllersManager.GetLeftAnalog(controllerId);
        bool fireBtn = controllersManager.GetRightTrigger(controllerId) > 0.25f;

        //Launching to hooks
        if (!isFiring && fireBtn)
        {
            isFiring = true;
            CacheHooks();
        }
        else if (isFiring)
        {
            if (aimDirection == Vector2.zero)
            {
                if (selectedHook)
                {
                    selectedHook.Deselect();
                    selectedHook = null;
                }
            }
            else
            {
                GameObject hookObj = GetHook(aimDirection);

                if (hookObj)
                {
                    Vector3 delta = hookObj.transform.position - transform.position;
                    float angle = Mathf.Atan2(delta.y, delta.x) * 180 / Mathf.PI;
                    Hook hookCmp = hookObj.GetComponent<Hook>();

                    //Rotate gun
                    gunObj.eulerAngles = new Vector3(0, 0, angle);

                    //Select hook
                    if (hookCmp != selectedHook)
                    {
                        if (selectedHook)
                            selectedHook.Deselect();

                        selectedHook = hookCmp;
                        selectedHook.Select();
                    }
                }
            }

            //Fire gun
            if (!fireBtn)
            {
                isFiring = false;

                if (selectedHook)
                {
                    selectedHook.Deselect();
                    selectedHook = null;

                    Debug.Log("Fire!");
                }
            }
        }
    }

    /*void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == hookTag)
        {
            if (selectedHook && other.gameObject != selectedHook.gameObject)
            {
                Debug.Log("Connected hook!");
            }
        }
    }*/

    void CacheHooks()
    {
        hooksCache = new AngleCollection();

        foreach (GameObject hookObj in GameObject.FindGameObjectsWithTag(hookTag))
        {
            if (Vector3.Distance(transform.position, hookObj.transform.position) < maxDistance)
            {
                Vector3 delta = hookObj.transform.position - transform.position;
                hooksCache.Add(hookObj, Mathf.Atan2(delta.y, delta.x) * 180 / Mathf.PI);
            }
        }
    }

    GameObject GetHook(Vector2 direction)
    {
        return hooksCache.GetClosestTo(Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI, maxAngle);
    }

    //Gameobjects with angles
    class AngleCollection
    {
        List<Tuple<GameObject, float>> objAngles = new List<Tuple<GameObject, float>>();

        public void Add(GameObject obj, float angle)
        {
            //Add hook to list
            objAngles.Add(Tuple.Create(obj, angle));
        }

        public GameObject GetClosestTo(float angle, float maxAngle = 30f)
        {
            //Find hook closest to angle
            Tuple<GameObject, float> closest = Tuple.Create<GameObject, float>(null, maxAngle);

            foreach (Tuple<GameObject, float> hookAngle in objAngles)
            {
                float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(angle, hookAngle.Item2));

                if (deltaAngle < closest.Item2)
                    closest = Tuple.Create(hookAngle.Item1, deltaAngle);
            }

            return closest.Item1;
        }
    }
}
