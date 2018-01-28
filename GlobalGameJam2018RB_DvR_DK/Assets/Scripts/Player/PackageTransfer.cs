using System;
using UnityEngine;

public class PackageTransfer : MonoBehaviour
{
    //Parameters
    public float maxDistance = 2.0f;

    //Private members
    bool isActing = false;
    RopeColor prevSelected;

    PlayerInput pInput;

    void Start()
    {
        pInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!pInput.IsActive())
            return;

        if (pInput.IsActing())
            isActing = true;

        if (isActing)
        {
            RopeManager.Rope closestRope = pInput.ropeManager.GetClosestRope(transform.position, maxDistance);

            if (!pInput.IsActing())
            {
                isActing = false;

                if (prevSelected)
                {
                    prevSelected.ResetColor();
                    prevSelected = null;
                }

                if (closestRope != null)
                    TransferPackage(closestRope);
            }
            else
            {
                if (closestRope != null)
                {
                    RopeColor selected = closestRope.lineRenderer.GetComponent<RopeColor>();

                    if (prevSelected != selected)
                    {
                        if (prevSelected)
                            prevSelected.ResetColor();

                        prevSelected = selected;
                        prevSelected.Highlight();
                    }
                }
                else if (prevSelected)
                {
                    prevSelected.ResetColor();
                    prevSelected = null;
                }
            }
        }
    }

    bool TransferPackage(RopeManager.Rope rope)
    {
        foreach (GameObject packageObj in GameObject.FindGameObjectsWithTag("Package"))
        {
            Package package = packageObj.GetComponent<Package>();

            if (package.ropeTransfer == null && package.hookedTo == rope.from.transform)
            {
                package.hookedTo = rope.to.transform;
                package.ropeTransfer = rope;
                rope.inTransfer++;

                if(rope.inTransfer == 1)
                    rope.lineRenderer.GetComponent<RopeColor>().Highlight();

                return true;
            }
        }

        return false;
    }
}
