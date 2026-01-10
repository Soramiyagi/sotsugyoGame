using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int maxHP = 20;
    protected int currentHP;

    // ★ protected のまま
    protected Animator animator;

    protected virtual void Start()
    {
        currentHP = maxHP;

        // ★ 子オブジェクトも含めて取得
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError($"{name}: Animator not found in children");
        }
    }

    public float HPRatio => (float)currentHP / maxHP;

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
            Die();
    }

    protected virtual void Die()
    {
        if (animator != null)
            animator.SetTrigger("death");

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        Destroy(gameObject, 0.5f);
    }
}
