using UnityEngine;
using System.Collections;

public class AdvancedEnemyContext2D : EnemyBase
{
    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Attack")]
    public float meleeRange = 2f;
    public float rangedRange = 8f;

    public Collider2D meleeCollider;
    public float meleeDuration = 0.5f;
    public float meleeCooldown = 10f;

    public EnemyBoomerangShooter boomerangShooter;
    public Transform firePoint;

    [Header("Facing (Pivot)")]
    public Transform flipPivot;
    public bool isLeftFacingDefault = true;

    [Header("Animation")]
    public Animator animator;

    [HideInInspector] public Rigidbody2D rb;

    private bool facingLeft;
    private bool isAttacking;

    public bool IsAttacking => isAttacking;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        facingLeft = isLeftFacingDefault;
        ApplyFacing();

        if (meleeCollider != null)
            meleeCollider.enabled = false;
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

    public void FaceByMoveDirection(float moveX)
    {
        if (flipPivot == null) return;
        if (Mathf.Abs(moveX) < 0.01f) return;

        bool shouldFaceLeft = moveX < 0f;
        if (shouldFaceLeft == facingLeft) return;

        facingLeft = shouldFaceLeft;
        ApplyFacing();
    }

    private void ApplyFacing()
    {
        float dir = facingLeft ? -1f : 1f;
        if (!isLeftFacingDefault) dir *= -1f;

        Vector3 s = flipPivot.localScale;
        s.x = Mathf.Abs(s.x) * dir;
        flipPivot.localScale = s;

        if (firePoint != null)
        {
            Vector3 p = firePoint.localPosition;
            p.x = Mathf.Abs(p.x) * (facingLeft ? -1f : 1f);
            firePoint.localPosition = p;
        }
    }

    // ===== ‹ßÚUŒ‚ =====
    public void DoMeleeAttack()
    {
        if (isAttacking || meleeCollider == null) return;

        Debug.Log("[AI] DoMeleeAttack ŠJŽn");

        if (animator != null)
            animator.SetTrigger("Melee");

        StartCoroutine(MeleeRoutine());
    }

    private IEnumerator MeleeRoutine()
    {
        isAttacking = true;
        rb.velocity = new Vector2(0f, rb.velocity.y);

        Debug.Log("[AI] Melee Collider ON");
        meleeCollider.enabled = true;

        yield return new WaitForSeconds(meleeDuration);

        meleeCollider.enabled = false;
        Debug.Log("[AI] Melee Collider OFF");

        yield return new WaitForSeconds(meleeCooldown);
        isAttacking = false;
    }

    // ===== ‰“‹——£UŒ‚ =====
    public void DoRangedAttack()
    {
        if (isAttacking || boomerangShooter == null) return;

        Debug.Log("[AI] DoRangedAttack ŠJŽn");

        isAttacking = true;
        rb.velocity = new Vector2(0f, rb.velocity.y);

        if (animator != null)
            animator.SetTrigger("Ranged");

        boomerangShooter.Shoot();
        StartCoroutine(RangedCooldown());
    }

    private IEnumerator RangedCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }
}
