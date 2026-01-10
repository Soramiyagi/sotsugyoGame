using UnityEngine;

public class EnemyMeleeHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 20;

    private bool hasHit;

    private void OnEnable()
    {
        // 攻撃開始時に必ずリセット
        hasHit = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Player"))
        {
            hasHit = true;

            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
