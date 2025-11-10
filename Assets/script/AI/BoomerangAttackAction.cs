using UnityEngine;

public class BoomerangAttackAction2D : UtilityAction2D
{
    public BoomerangAttackAction2D()
    {
        var near = new DistanceNearConsideration(); // 引数なしで生成
        near.effectiveRange = 15f;                  // 射程を指定
        considerations.Add(near);
    }

    public override void Execute(EnemyContext2D ctx)
    {
        if (ctx == null || ctx.boomerangShooter == null || ctx.player == null) return;

        float distance = ctx.DistanceToPlayer();

        if (distance <= ctx.attackRange)
        {
            ctx.rb.velocity = new Vector2(0f, ctx.rb.velocity.y);
            // 必要に応じて向きやアニメーション処理
        }
    }
}
