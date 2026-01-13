using UnityEngine;
using System.Collections;

public class EnemyBoomerangShooter : MonoBehaviour
{
    public GameObject boomerangPrefab;
    public Transform firePoint;
    public float attackCooldown = 3f;
    public float attackRange = 15f;

    private Transform player;
    private float cooldownTimer = 0f;
    private bool isInRange = false;
    private bool isAttacking = false;

    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponentInChildren<Animator>();
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
                TryShoot();
                cooldownTimer = 0f;
            }
            else
            {
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer >= attackCooldown)
                {
                    cooldownTimer = 0f;
                    TryShoot();
                }
            }
        }

        isInRange = currentlyInRange;
    }

    public void TryShoot()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger("IsBoomerang");

        // ★ 発射を0.1秒遅らせる
        StartCoroutine(ShootAfterDelay(0.1f));
    }

    public void Shoot()
    {
        TryShoot();
        Debug.Log("[Boomerang] Shoot() 呼ばれた");

        if (firePoint == null)
        {
            Debug.LogError("[Boomerang] firePoint が null");
            return;
        }

        if (boomerangPrefab == null)
        {
            Debug.LogError("[Boomerang] prefab が null");
            return;
        }

        Debug.Log("[Boomerang] Instantiate 実行");

        Instantiate(boomerangPrefab, firePoint.position, Quaternion.identity);
    }

    IEnumerator ShootAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject boomerang = Instantiate(boomerangPrefab, firePoint.position, Quaternion.identity);
        BoomerangAttackFromEnemy script = boomerang.GetComponent<BoomerangAttackFromEnemy>();

        if (script == null)
        {
            Debug.LogError("BoomerangAttackFromEnemy がアタッチされていません！");
            yield break;
        }

        script.Initialize(player, this.transform);
    }

    // アニメーションイベントから呼ばれる
    public void OnBoomerangAttackEnd()
    {
        isAttacking = false;
    }
}

