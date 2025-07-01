using UnityEngine;

public class EnemyBoomerangShooter : MonoBehaviour
{
    public GameObject boomerangPrefab;
    public Transform firePoint;
    public float attackCooldown = 3f;
    public float attackRange = 15f;

    private Transform player;
    private float cooldownTimer = 0f;
    private bool isInRange = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool currentlyInRange = distanceToPlayer <= attackRange;

        if (currentlyInRange)
        {
            if (!isInRange)
            {
                // ★ 初めて範囲に入った瞬間：即撃つ！
                ShootBoomerang();
                cooldownTimer = 0f; // クールタイムリセット
            }
            else
            {
                // ★ 範囲内にいる間：クールタイムで撃つ
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer >= attackCooldown)
                {
                    cooldownTimer = 0f;
                    ShootBoomerang();
                }
            }
        }

        isInRange = currentlyInRange; // 状態更新
    }

    void ShootBoomerang()
    {
        GameObject boomerang = Instantiate(boomerangPrefab, firePoint.position, Quaternion.identity);
        BoomerangAttackFromEnemy script = boomerang.GetComponent<BoomerangAttackFromEnemy>();

        if (script == null)
        {
            Debug.LogError("BoomerangAttackFromEnemy がアタッチされていません！");
            return;
        }

        script.Initialize(player, this.transform);
    }
}


