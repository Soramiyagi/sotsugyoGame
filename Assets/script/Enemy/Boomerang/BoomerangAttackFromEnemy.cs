using UnityEngine;

public class BoomerangAttackFromEnemy : MonoBehaviour
{
    public float speed = 10f;
    public float returnDelay = 1.5f;
    public int damage = 20;

    private Transform playerTarget;
    private Transform returnTarget;
    private Rigidbody2D rb;
    private bool isReturning = false;
    private float timer = 0f;

    public void Initialize(Transform player, Transform enemy)
    {
        playerTarget = player;
        returnTarget = enemy;

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D がついていません！");
            return;
        }

        // プレイヤー方向に +Y補正を追加して狙う
        Vector2 direction = (player.position - transform.position);
        direction.y += 2f; //  Y方向補正
        direction = direction.normalized;
        Debug.Log("A");
        rb.velocity = direction * speed;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!isReturning && timer >= returnDelay)
        {
            isReturning = true;
        }

        if (isReturning && returnTarget != null)
        {
            Vector2 direction = (returnTarget.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning && collision.transform == returnTarget)
        {
            Destroy(gameObject);
        }
    }
}


