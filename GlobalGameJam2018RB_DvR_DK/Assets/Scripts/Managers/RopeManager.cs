using System.Collections.Generic;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    List<Rope> ropes = new List<Rope>();

    void Start()
    {

    }

    void Update()
    {

    }

    /*public GameObject GetClosestRope(Vector3 pos, float maxDistance = 10.0f)
    {

    }*/

    public class Rope
    {
        PlayerInput owner;
        LineRenderer lineRenderer;
        bool inTransfer;
    }
}
