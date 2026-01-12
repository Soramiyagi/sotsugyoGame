using UnityEngine;

public class AdvancedRangedAction2D : AdvancedUtilityAction2D
{
    public AdvancedRangedAction2D()
    {
        considerations.Add(new InRange());
        considerations.Add(new Height());
    }

    public override void Execute(AdvancedEnemyContext2D ctx)
    {
        if (ctx.IsAttacking) return;

        if (ctx.DistanceToPlayer() <= ctx.rangedRange)
        {
            Debug.Log("[AI] Ranged Execute ŒÄ‚Ño‚µ");
            ctx.DoRangedAttack();
        }
    }

    class InRange : IAdvancedConsideration2D
    {
        public float Score(AdvancedEnemyContext2D ctx)
        {
            return Mathf.Clamp01(ctx.DistanceToPlayer() / ctx.rangedRange) * 1.4f;
        }
    }

    class Height : IAdvancedConsideration2D
    {
        public float Score(AdvancedEnemyContext2D ctx)
        {
            float diff = ctx.player.position.y - ctx.transform.position.y;
            return Mathf.Clamp01(diff / 3f) * 1.3f;
        }
    }
}
