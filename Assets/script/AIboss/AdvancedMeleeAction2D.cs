using UnityEngine;

public class AdvancedMeleeAction2D : AdvancedUtilityAction2D
{
    public AdvancedMeleeAction2D()
    {
        considerations.Add(new MeleeDistanceConsideration());
        considerations.Add(new HighHPConsideration());
    }

    public override void Execute(AdvancedEnemyContext2D ctx)
    {
        if (ctx.DistanceToPlayer() <= ctx.meleeRange)
        {
            ctx.rb.velocity = new Vector2(0, ctx.rb.velocity.y);
            // ここにコリジョン表示・アニメーション
        }
    }

    class MeleeDistanceConsideration : IAdvancedConsideration2D
    {
        public float Score(AdvancedEnemyContext2D ctx)
        {
            float d = ctx.DistanceToPlayer();
            return 1f - Mathf.Clamp01(d / ctx.meleeRange) * 1.5f; // 優先度強化
        }
    }

    class HighHPConsideration : IAdvancedConsideration2D
    {
        public float Score(AdvancedEnemyContext2D ctx)
        {
            return ctx.HPRatio * 1.2f;
        }
    }
}
