using UnityEngine;

public class BoomerangAttackAction2D : UtilityAction2D
{
    public BoomerangAttackAction2D()
    {
        var near = new DistanceNearConsideration();
        near.effectiveRange = 15f;
        considerations.Add(near);
    }

    public override void Execute(EnemyContext2D ctx)
    {
        if (ctx == null || ctx.boomerangShooter == null || ctx.player == null)
            return;

        float distance = ctx.DistanceToPlayer();

        if (distance <= ctx.attackRange)
        {
            // ˆÚ“®’âŽ~
            ctx.rb.velocity = new Vector2(0f, ctx.rb.velocity.y);

            // š ‚±‚ê‚ð’Ç‰Á
            ctx.boomerangShooter.Shoot();
        }
    }
}
