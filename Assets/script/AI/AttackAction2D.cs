using UnityEngine;

public class AttackAction2D : UtilityAction2D
{
    private float lastAttackTime;
    private float cooldown = 1f;

    public AttackAction2D()
    {
        considerations.Add(new DistanceNearConsideration());
    }

    public override void Execute(EnemyContext2D ctx)
    {
        if (Time.time - lastAttackTime < cooldown) return;

        float dist = ctx.DistanceToPlayer();
        if (dist <= ctx.attackRange)
        {
            // 物理的な移動は止める
            ctx.rb.velocity = new Vector2(0, ctx.rb.velocity.y);
            Debug.Log($"{ctx.name} attacks!");
            lastAttackTime = Time.time;

            // ここにアニメーションや弾生成など追加可能
        }
    }
}

public class DistanceNearConsideration : IConsideration2D
{
    public float effectiveRange = 2f;

    public float Score(EnemyContext2D ctx)
    {
        float dist = ctx.DistanceToPlayer();
        return 1f - Mathf.Clamp01(dist / effectiveRange); // 近いほどスコアが高い
    }
}

