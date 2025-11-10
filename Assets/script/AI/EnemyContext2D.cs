using UnityEngine;



[RequireComponent(typeof(Rigidbody2D))]
public class EnemyContext2D : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float attackRange = 15f;
    public EnemyBoomerangShooter boomerangShooter;

    [HideInInspector] public Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
}


