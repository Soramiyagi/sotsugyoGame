using UnityEngine;

public class AdvancedChaseAction2D : AdvancedUtilityAction2D
{
    public AdvancedChaseAction2D()
    {
        considerations.Add(new DistanceFarConsideration());
    }

    public override void Execute(AdvancedEnemyContext2D ctx)
    {
        Vector2 dir = ctx.DirectionToPlayer();
        Vector2 targetVelocity = dir * ctx.moveSpeed;
        targetVelocity.y = ctx.rb.velocity.y;
        ctx.rb.velocity = Vector2.Lerp(ctx.rb.velocity, targetVelocity, 0.1f);

        if (dir.x != 0)
        {
            bool faceLeft = dir.x < 0;
            ctx.spriteRenderer.flipX = faceLeft;

            Vector3 fp = ctx.firePoint.localPosition;
            fp.x = Mathf.Abs(fp.x) * (faceLeft ? -1 : 1);
            ctx.firePoint.localPosition = fp;
        }
    }

    class DistanceFarConsideration : IAdvancedConsideration2D
    {
        public float maxDistance = 15f;
        public float Score(AdvancedEnemyContext2D ctx)
        {
            float dist = ctx.DistanceToPlayer();
            return Mathf.Clamp01(dist / maxDistance) * 0.8f; // ˆÚ“®‚Í­‚µ—Dæ“x‰º‚°
        }
    }
}
