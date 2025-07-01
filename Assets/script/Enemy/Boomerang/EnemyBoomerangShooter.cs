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
                // �� ���߂Ĕ͈͂ɓ������u�ԁF�����I
                ShootBoomerang();
                cooldownTimer = 0f; // �N�[���^�C�����Z�b�g
            }
            else
            {
                // �� �͈͓��ɂ���ԁF�N�[���^�C���Ō���
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer >= attackCooldown)
                {
                    cooldownTimer = 0f;
                    ShootBoomerang();
                }
            }
        }

        isInRange = currentlyInRange; // ��ԍX�V
    }

    void ShootBoomerang()
    {
        GameObject boomerang = Instantiate(boomerangPrefab, firePoint.position, Quaternion.identity);
        BoomerangAttackFromEnemy script = boomerang.GetComponent<BoomerangAttackFromEnemy>();

        if (script == null)
        {
            Debug.LogError("BoomerangAttackFromEnemy ���A�^�b�`����Ă��܂���I");
            return;
        }

        script.Initialize(player, this.transform);
    }
}


