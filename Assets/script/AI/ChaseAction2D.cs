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
        Vector2 targetVelocity = dir * ctx.moveSpeed;

        // x•ûŒü‚Ì‚İˆÚ“®
        targetVelocity.y = ctx.rb.velocity.y;

        ctx.rb.velocity = Vector2.Lerp(ctx.rb.velocity, targetVelocity, 0.1f);
    }
}

public class DistanceFarConsideration : IConsideration2D
{
    public float maxDistance = 8f;

    public float Score(EnemyContext2D ctx)
    {
        float dist = ctx.DistanceToPlayer();
        return Mathf.Clamp01(dist / maxDistance); // ‰“‚¢‚Ù‚ÇƒXƒRƒA‚ª‚‚¢
    }
}

