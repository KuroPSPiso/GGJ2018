using UnityEngine;

public class EndCentre : Hook
{
    public Transform endPoint;
    public int packageType;
    public Transform tankFill;
    float score = 0.0f;
    public PlayerManager playerManager;

    public float maxScore = 3;
    bool winning = false;

    void Update()
    {
        if (tankFill.localScale.z < score * 13 && !winning)
        {
            Vector3 scale = tankFill.localScale;
            scale.z = Mathf.Min(13, scale.z + Time.deltaTime * 2.0f);
            tankFill.localScale = scale;

            if (score >= 1.0f)
            {
                playerManager.GotoWin(packageType);
                winning = true;
            }
        }
    }

    public void IncreaseData()
    {
        score += 1.0f / maxScore;
        AudioManager.TankFilled();
    }
}
