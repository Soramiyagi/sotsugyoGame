using System.Collections.Generic;
using UnityEngine;

public abstract class AdvancedUtilityAction2D
{
    protected List<IAdvancedConsideration2D> considerations = new();

    public float GetScore(AdvancedEnemyContext2D ctx)
    {
        if (considerations.Count == 0) return 0f;

        float score = 1f;
        foreach (var c in considerations)
            score *= Mathf.Clamp01(c.Score(ctx));

        return score;
    }

    public abstract void Execute(AdvancedEnemyContext2D ctx);
}
