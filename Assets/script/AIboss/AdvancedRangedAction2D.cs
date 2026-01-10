using UnityEngine;

public class AdvancedRangedAction2D : AdvancedUtilityAction2D
{
    public AdvancedRangedAction2D()
    {
        considerations.Add(new DistanceConsideration());
        considerations.Add(new HeightConsideration());
    }

    public override void Execute(AdvancedEnemyContext2D ctx)
    {
        if (ctx.IsAttacking) return;

        float dist = ctx.DistanceToPlayer();

        if (dist <= ctx.rangedRange && ctx.boomerangShooter != null)
        {
            Debug.Log("[AI] Ranged Execute ŒÄ‚Ño‚µ");

            ctx.rb.velocity = new Vector2(0f, ctx.rb.velocity.y);
            ctx.boomerangShooter.Shoot();
        }
    }

    class DistanceConsideration : IAdvancedConsideration2D
    {
        public float Score(AdvancedEnemyContext2D ctx)
        {
            float d = ctx.DistanceToPlayer();
            return Mathf.Clamp01(d / ctx.rangedRange) * 1.5f;
        }
    }

    class HeightConsideration : IAdvancedConsideration2D
    {
        public float Score(AdvancedEnemyContext2D ctx)
        {
            if (ctx.player == null) return 0f;
            float diff = ctx.player.position.y - ctx.transform.position.y;
            return Mathf.Clamp01(diff / 3f);
        }
    }
}
