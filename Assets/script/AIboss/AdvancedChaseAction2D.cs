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

        Vector2 v = dir * ctx.moveSpeed;
        v.y = ctx.rb.velocity.y;
        ctx.rb.velocity = Vector2.Lerp(ctx.rb.velocity, v, 0.1f);

        ctx.FaceByMoveDirection(ctx.rb.velocity.x);
    }

    class DistanceFarConsideration : IAdvancedConsideration2D
    {
        public float maxDistance = 15f;

        public float Score(AdvancedEnemyContext2D ctx)
        {
            float dist = ctx.DistanceToPlayer();
            return Mathf.Clamp01(dist / maxDistance) * 0.6f;
        }
    }
}
