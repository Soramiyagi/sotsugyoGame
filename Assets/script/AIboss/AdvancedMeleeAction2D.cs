using UnityEngine;

public class AdvancedMeleeAction2D : AdvancedUtilityAction2D
{
    public AdvancedMeleeAction2D()
    {
        considerations.Add(new Near());
        considerations.Add(new HighHP());
    }

    public override void Execute(AdvancedEnemyContext2D ctx)
    {
        if (ctx.IsAttacking) return;

        if (ctx.DistanceToPlayer() <= ctx.meleeRange)
        {
            Debug.Log("[AI] Melee Execute ŒÄ‚Ño‚µ");
            ctx.DoMeleeAttack();
        }
    }

    class Near : IAdvancedConsideration2D
    {
        public float Score(AdvancedEnemyContext2D ctx)
        {
            return Mathf.Clamp01(1f - ctx.DistanceToPlayer() / ctx.meleeRange) * 1.6f;
        }
    }

    class HighHP : IAdvancedConsideration2D
    {
        public float Score(AdvancedEnemyContext2D ctx)
        {
            return ctx.HPRatio;
        }
    }
}
