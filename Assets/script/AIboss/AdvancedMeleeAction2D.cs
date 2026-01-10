using UnityEngine;

public class AdvancedMeleeAction2D : AdvancedUtilityAction2D
{
    public AdvancedMeleeAction2D()
    {
        considerations.Add(new DistanceConsideration());
    }

    public override void Execute(AdvancedEnemyContext2D ctx)
    {
        if (ctx.IsAttacking) return;

        if (ctx.DistanceToPlayer() <= ctx.meleeRange)
        {
            Debug.Log("[AI] Melee Execute");
            ctx.DoMeleeAttack();
        }
    }

    class DistanceConsideration : IAdvancedConsideration2D
    {
        public float Score(AdvancedEnemyContext2D ctx)
        {
            float d = ctx.DistanceToPlayer();
            return Mathf.Clamp01(1f - d / ctx.meleeRange) * 1.5f;
        }
    }
}
