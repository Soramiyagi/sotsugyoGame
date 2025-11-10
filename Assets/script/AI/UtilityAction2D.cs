using System.Collections.Generic;
using UnityEngine;

public abstract class UtilityAction2D
{
    protected List<IConsideration2D> considerations = new List<IConsideration2D>();

    public float GetScore(EnemyContext2D ctx)
    {
        if (considerations.Count == 0) return 0f;

        float total = 1f;
        foreach (var c in considerations)
            total *= Mathf.Clamp01(c.Score(ctx));

        return total;
    }

    public abstract void Execute(EnemyContext2D ctx);
}

