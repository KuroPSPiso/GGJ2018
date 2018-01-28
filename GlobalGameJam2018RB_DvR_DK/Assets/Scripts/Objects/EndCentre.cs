using UnityEngine;

public class EndCentre : Hook
{
    public Transform endPoint;
    public int packageType;
    public Transform tankFill;
    float score = 0.0f;

	public float maxScore = 3;

    void Update()
    {
        if (tankFill.localScale.z < score * 13)
        {
            Vector3 scale = tankFill.localScale;
            scale.z += Time.deltaTime;
            tankFill.localScale = scale;

            if (scale.z >= score * 13)
            {
                //TODO:
                Debug.Log("winning team is " + packageType);
            }
        }
    }

    public void IncreaseData()
    {
		score += 1.0f / maxScore; 
    }
}
