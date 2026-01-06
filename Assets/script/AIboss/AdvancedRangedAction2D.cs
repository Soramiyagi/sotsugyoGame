using UnityEngine;

public class AdvancedRangedAction2D : AdvancedUtilityAction2D
{
    public AdvancedRangedAction2D()
    {
        considerations.Add(new RangedDistanceConsideration());
        considerations.Add(new HeightConsideration());
    }

    public override void Execute(AdvancedEnemyContext2D ctx)
    {
        if (ctx.DistanceToPlayer() <= ctx.rangedRange && ctx.boomerangShooter != null)
        {
            ctx.rb.velocity = new Vector2(0, ctx.rb.velocity.y);
            ctx.boomerangShooter.Shoot(); // 公開メソッドを使用
        }
    }

    class RangedDistanceConsideration : IAdvancedConsideration2D
    {
        public float Score(AdvancedEnemyContext2D ctx)
        {
            float d = ctx.DistanceToPlayer();
            return Mathf.Clamp01(d / ctx.rangedRange) * 1.3f;
        }
    }

    class HeightConsideration : IAdvancedConsideration2D
    {
        public float Score(AdvancedEnemyContext2D ctx)
        {
            float diff = ctx.player.position.y - ctx.transform.position.y;
            return Mathf.Clamp01(diff / 3f) * 1.5f;
        }
    }
}
