using System.Collections.Generic;
using UnityEngine;

public class RopeManager : MonoBehaviour
{
    List<Rope> ropes = new List<Rope>();

    public Rope GetClosestRope(Vector3 pos, float maxDistance = 2.0f)
    {
        KeyValuePair<Rope, float> closest = new KeyValuePair<Rope, float>(null, maxDistance);

        foreach (Rope rope in ropes)
        {
            if (rope.inTransfer == 0)
            {
                Vector3 from = rope.lineRenderer.GetPosition(0);
                Vector3 to = rope.lineRenderer.GetPosition(1);

                float distance = Vector3.Distance(NearestPointOnLine(from, to, pos), pos);

                if (distance < closest.Value)
                    closest = new KeyValuePair<Rope, float>(rope, distance);
            }
        }

        return closest.Key;
    }

    public bool IsHookConnected(Hook hook)
    {
        foreach (Rope rope in ropes)
        {
            if (rope.from == hook || rope.to == hook)
                return true;
        }

        return false;
    }

    //Find nearest point on a line
    static Vector3 NearestPointOnLine(Vector3 start, Vector3 end, Vector3 pnt)
    {
        Vector3 line = (end - start);
        float len = line.magnitude;
        line.Normalize();

        Vector3 v = pnt - start;
        float d = Vector3.Dot(v, line);
        d = Mathf.Clamp(d, 0f, len);

        return start + line * d;
    }

    public void AddRope(PlayerInput owner, LineRenderer ropeLine, Hook from, Hook to)
    {
        ropes.Add(new Rope(owner, ropeLine, from, to));
    }

    public bool TrimRopes(PlayerInput owner, int maxRopes = 2)
    {
        if (CountRopes(owner) < maxRopes)
            return true;

        foreach (Rope rope in ropes)
        {
            if (rope.owner == owner)
            {
                if (rope.inTransfer == 0)
                {
                    Destroy(rope.lineRenderer.gameObject);
                    ropes.Remove(rope);
                    return true;
                }
            }
        }

        return false;
    }

    int CountRopes(PlayerInput owner)
    {
        int count = 0;

        foreach (Rope rope in ropes)
        {
            if (rope.owner == owner)
                count++;
        }

        return count;
    }

    public class Rope
    {
        public PlayerInput owner;
        public LineRenderer lineRenderer;
        public Hook from, to;
        public int inTransfer;

        public Rope(PlayerInput owner, LineRenderer lineRenderer, Hook from, Hook to)
        {
            this.owner = owner;
            this.lineRenderer = lineRenderer;
            this.from = from;
            this.to = to;

            inTransfer = 0;
        }
    }
}
