using UnityEngine;

public class ChaseAction2D : UtilityAction2D
{
    public ChaseAction2D()
    {
        considerations.Add(new DistanceFarConsideration());
    }

    public override void Execute(EnemyContext2D ctx)
    {
        Vector2 dir = ctx.DirectionToPlayer();

        // 移動処理
        Vector2 targetVelocity = dir * ctx.moveSpeed;
        targetVelocity.y = ctx.rb.velocity.y;
        ctx.rb.velocity = Vector2.Lerp(ctx.rb.velocity, targetVelocity, 0.1f);

        // 向き変更
        if (dir.x != 0)
        {
            bool faceLeft = dir.x < 0;

            // スプライト反転
            ctx.spriteRenderer.flipX = faceLeft;

            // FirePoint 反転
            Vector3 fp = ctx.firePoint.localPosition;
            fp.x = Mathf.Abs(fp.x) * (faceLeft ? -1 : 1);
            ctx.firePoint.localPosition = fp;
        }
    }

    public class DistanceFarConsideration : IConsideration2D
    {
        public float maxDistance = 15f;

        public float Score(EnemyContext2D ctx)
        {
            float dist = ctx.DistanceToPlayer();
            return Mathf.Clamp01(dist / maxDistance);
        }
    }
}


