using UnityEngine;

public class Package : MonoBehaviour
{
    //Parameter
    public float rotateSpeed = 100.0f;
    public float rotateDistance = 1.0f;
    public float moveSpeed = 3.0f;
    public float movingToEndTime = 3.0f;

    //Public members
    public Transform hookedTo;
    public bool inTransfer = true;
    public RopeManager.Rope ropeTransfer;
    EndCentre movingToEnd;
    
    public int packageType;

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, hookedTo.position);

        if (distanceToTarget > rotateDistance)
        {
            Vector3 moveDir = (hookedTo.position - transform.position).normalized;
            transform.position += moveDir * Mathf.Min(moveSpeed * Time.deltaTime, distanceToTarget);
        }
        else
        {
            transform.RotateAround(hookedTo.position, Vector3.forward, rotateSpeed * Time.deltaTime);

            if (movingToEnd)
            {
                if (movingToEndTime <= 0.0f)
                    Destroy(gameObject);
                else
                {
                    movingToEndTime -= Time.deltaTime;

                    if (movingToEndTime <= 0.0f)
                    {
                        hookedTo = movingToEnd.endPoint;
                        movingToEnd.IncreaseData();
                    }
                }
            }
            else if (ropeTransfer != null)
            {
                EndCentre endCentre = ropeTransfer.to as EndCentre;

                if (endCentre && endCentre.packageType == packageType)
                    movingToEnd = endCentre;
                else
                    inTransfer = false;

                ropeTransfer.inTransfer--;
                ropeTransfer = null;
            }
        }
    }
}
