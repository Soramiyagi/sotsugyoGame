using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int maxHP = 100;    // ç≈ëÂHP
    protected int currentHP;

    protected virtual void Start()
    {
        currentHP = maxHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack"))
        {
            TakeDamage(20);
            Debug.Log("Enemy hit!");
        }
    }

    protected virtual void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}

