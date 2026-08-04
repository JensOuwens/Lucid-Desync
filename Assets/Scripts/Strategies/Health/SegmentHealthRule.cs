using UnityEngine;

public class SegmentHealthRule : IHealthRule
{
    private int maxSegments;
    private int currentSegments;

    public SegmentHealthRule(int maxSegments)
    {
        this.maxSegments =  maxSegments;
        
        currentSegments = maxSegments;
    }
    
    public void Damage(float damage)
    {
        if (damage < 0)
        {
            currentSegments++;
            if (currentSegments > maxSegments)
            {
                currentSegments = maxSegments;
            }
        }
        else if (damage > 0)
        {
            currentSegments--;
        }

        if (currentSegments <= 0)
        {
            currentSegments = 0;
            Die();
        }
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public float ReturnHealth()
    {
        return currentSegments;
    }
}
