using UnityEngine;

public class Package : MonoBehaviour
{
    //Parameter
    public float rotateSpeed = 100.0f;
    public float rotateDistance = 1.0f;
    public float moveSpeed = 3.0f;

    //Public members
    public Transform hookedTo;
    public RopeManager.Rope ropeTransfer;
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

            if (ropeTransfer != null)
            {
                ropeTransfer.inTransfer--;
                ropeTransfer = null;
            }
        }
    }
}
