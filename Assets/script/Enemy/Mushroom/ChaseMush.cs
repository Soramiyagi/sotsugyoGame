using UnityEngine;

public class ChaseMush : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform player;
    public Transform firePoint;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            SetMoving(false);
            return;
        }

        // プレイヤーへの方向
        Vector2 dir = (player.position - transform.position).normalized;

        // X方向に動いているか
        bool isMoving = Mathf.Abs(dir.x) > 0.01f;

        // 移動
        Vector2 targetVelocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
        rb.velocity = Vector2.Lerp(rb.velocity, targetVelocity, 0.1f);

        // Animator
        SetMoving(isMoving);

        // 向き変更
        if (isMoving)
        {
            bool faceLeft = dir.x < 0;
            spriteRenderer.flipX = faceLeft;

            if (firePoint != null)
            {
                Vector3 fp = firePoint.localPosition;
                fp.x = Mathf.Abs(fp.x) * (faceLeft ? -1 : 1);
                firePoint.localPosition = fp;
            }
        }
    }

    void SetMoving(bool value)
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", value);
        }
    }
}
