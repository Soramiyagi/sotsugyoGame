using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int maxHP = 20;

    // ★ protected に変更（子クラスから参照可能）
    protected int currentHP;

    protected Animator animator;

    protected virtual void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack"))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("death");
        }

        rbStop();
        Destroy(gameObject, 0.5f);
    }

    void rbStop()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }
    }

    // ★ HP参照用（AIが使う）
    public float HPRatio => Mathf.Clamp01((float)currentHP / maxHP);
}
