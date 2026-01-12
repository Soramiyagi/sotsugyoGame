using UnityEngine;

public class AdvancedChaseAction2D : AdvancedUtilityAction2D
{
    public AdvancedChaseAction2D()
    {
        considerations.Add(new DistanceFar());
    }

    public override void Execute(AdvancedEnemyContext2D ctx)
    {
        Vector2 dir = ctx.DirectionToPlayer();

        Vector2 v = dir * ctx.moveSpeed;
        v.y = ctx.rb.velocity.y;
        ctx.rb.velocity = Vector2.Lerp(ctx.rb.velocity, v, 0.1f);

        ctx.FaceByMoveDirection(v.x);
    }

    class DistanceFar : IAdvancedConsideration2D
    {
        public float Score(AdvancedEnemyContext2D ctx)
        {
            return Mathf.Clamp01(ctx.DistanceToPlayer() / 10f) * 0.7f;
        }
    }
}
