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

    [Tooltip("近接攻撃用コリジョン（子オブジェクト）")]
    public Collider2D meleeCollider;

    [Tooltip("近接コリジョンの有効時間")]
    public float meleeDuration = 0.2f;

    [Tooltip("近接攻撃のクールタイム")]
    public float meleeCooldown = 3f;

    public EnemyBoomerangShooter boomerangShooter;
    public Transform firePoint;

    [Header("Facing (Pivot)")]
    public Transform flipPivot;

    [Tooltip("ON = 元画像は左向き / OFF = 元画像は右向き")]
    public bool isLeftFacingDefault = true;

    [Header("Animation")]
    public Animator animator;

    [HideInInspector] public Rigidbody2D rb;

    // ===== 内部状態 =====
    private bool facingLeft;
    private bool isAttacking;
    private float lastMeleeTime = -999f; // ★ 追加

    public bool IsAttacking => isAttacking;

    // =========================

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

    // =========================
    // 情報取得
    // =========================

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

    // =========================
    // 向き制御
    // =========================

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

        Vector3 scale = flipPivot.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        flipPivot.localScale = scale;

        if (firePoint != null)
        {
            Vector3 p = firePoint.localPosition;
            p.x = Mathf.Abs(p.x) * (facingLeft ? -1f : 1f);
            firePoint.localPosition = p;
        }
    }

    // =========================
    // 近接攻撃（★連打完全防止）
    // =========================

    public void DoMeleeAttack()
    {
        // ★ クールタイム判定（最重要）
        if (Time.time < lastMeleeTime + meleeCooldown)
            return;

        if (isAttacking || meleeCollider == null)
            return;

        lastMeleeTime = Time.time;

        Debug.Log("[AI] DoMeleeAttack 開始");

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

        isAttacking = false;
    }

    // =========================
    // 遠距離攻撃（既存）
    // =========================

    public void DoRangedAttack()
    {
        if (isAttacking || boomerangShooter == null) return;

        Debug.Log("[AI] DoRangedAttack 開始");

        isAttacking = true;
        rb.velocity = new Vector2(0f, rb.velocity.y);

        if (animator != null)
            animator.SetTrigger("Ranged");

        boomerangShooter.Shoot();

        StartCoroutine(RangedCooldown(0.5f));
    }

    private IEnumerator RangedCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        isAttacking = false;
    }
}
