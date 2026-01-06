using UnityEngine;

public class AdvancedEnemyContext2D : EnemyBase
{
    public Transform player;
    public float moveSpeed = 3f;
    public float meleeRange = 2f;
    public float rangedRange = 15f;
    public EnemyBoomerangShooter boomerangShooter;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public Transform firePoint;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public float DistanceToPlayer()
    {
        if (player == null) return float.MaxValue;
        return Vector2.Distance(transform.position, player.position);
    }

    public Vector2 DirectionToPlayer()
    {
        if (player == null) return Vector2.zero;
        return (player.position - transform.position).normalized;
    }

    public float HPRatio => (float)currentHP / maxHP;
}
