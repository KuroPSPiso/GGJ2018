using System;
using System.Collections.Generic;
using UnityEngine;

public class RopeLauncher : MonoBehaviour
{
    //Parameters
    public float maxDistance = 10f;
    public float maxAngle = 30f;
    public int maxRopes = 2;
    public Transform gunObj;
    public GameObject rope;

    //Private members
    AngleCollection hooksCache;
    float lastCacheTime;

    bool isFiring = false;
    LineRenderer ropeShot;
    bool ropeEnabled;
    Hook selectedHook;

    List<LineRenderer> ropes = new List<LineRenderer>();

    PlayerInput pInput;

    void Start()
    {
        pInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!pInput.IsActive())
            return;

        //Get aiming direction
        Vector2 aimDirection = pInput.GetAiming();

        //Launching to hooks
        if (!isFiring && pInput.IsFiring())
        {
            isFiring = true;
            CacheHooks();

            if (ropeShot)
            {
                Destroy(ropeShot);

                selectedHook = null;
                ropeShot = null;
            }
        }
        else if (isFiring)
        {
            if (Time.time - lastCacheTime > 0.5f)
                CacheHooks();

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
                    Vector3 delta = hookObj.transform.position - gunObj.position;
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
            if (!pInput.IsFiring())
            {
                isFiring = false;

                if (selectedHook)
                {
                    //Spawn rope
                    if (pInput.ropeManager.TrimRopes(pInput, maxRopes))
                    {
                        ropeEnabled = true;

                        ropeShot = Instantiate(rope).GetComponent<LineRenderer>();
                        ropeShot.SetPosition(1, selectedHook.transform.position);

                        selectedHook.Deselect();
                    }
                }
            }
        }

        //Update rope
        if (ropeShot)
        {
            ropeShot.SetPosition(0, gunObj.position);
            float distance = Vector3.Distance(gunObj.position, selectedHook.transform.position);

            if (ropeEnabled && distance > maxDistance)
            {
                ropeShot.GetComponent<RopeColor>().Disable();
                ropeEnabled = false;
            }
            else if (!ropeEnabled && distance < maxDistance)
            {
                ropeShot.GetComponent<RopeColor>().ResetColor();
                ropeEnabled = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hook")
        {
            if (ropeShot && other.gameObject != selectedHook.gameObject)
            {
                float distance = Vector3.Distance(selectedHook.transform.position, other.transform.position);

                if (distance < maxDistance)
                {
                    //Attach rope between hooks
                    LineRenderer ropeObj = Instantiate(rope).GetComponent<LineRenderer>();
                    ropeObj.SetPosition(0, other.transform.position);
                    ropeObj.SetPosition(1, selectedHook.transform.position);

                    pInput.ropeManager.AddRope(pInput, ropeObj, selectedHook, other.GetComponent<Hook>());

                    //Remove shot rope
                    Destroy(ropeShot);

                    selectedHook = null;
                    ropeShot = null;
                }
            }
        }
    }

    void CacheHooks()
    {
        hooksCache = new AngleCollection();
        lastCacheTime = Time.time;

        foreach (GameObject hookObj in GameObject.FindGameObjectsWithTag("Hook"))
        {
            if (Vector3.Distance(gunObj.position, hookObj.transform.position) < maxDistance)
            {
                if (!Physics2D.Linecast(gunObj.position, hookObj.transform.position, 1 << LayerMask.NameToLayer("Ground")))
                {
                    Vector3 delta = hookObj.transform.position - gunObj.position;
                    hooksCache.Add(hookObj, Mathf.Atan2(delta.y, delta.x) * 180 / Mathf.PI);
                }
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
        List<KeyValuePair<GameObject, float>> objAngles = new List<KeyValuePair<GameObject, float>>();

        public void Add(GameObject obj, float angle)
        {
            //Add hook to list
            objAngles.Add(new KeyValuePair<GameObject, float>(obj, angle));
        }

        public GameObject GetClosestTo(float angle, float maxAngle = 30f)
        {
            //Find hook closest to angle
            KeyValuePair<GameObject, float> closest = new KeyValuePair<GameObject, float>(null, maxAngle);

            foreach (KeyValuePair<GameObject, float> hookAngle in objAngles)
            {
                float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(angle, hookAngle.Value));

                if (deltaAngle < closest.Value)
                    closest = new KeyValuePair<GameObject, float>(hookAngle.Key, deltaAngle);
            }

            return closest.Key;
        }
    }
}
