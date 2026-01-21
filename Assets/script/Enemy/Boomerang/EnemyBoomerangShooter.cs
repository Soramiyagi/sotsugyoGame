using UnityEngine;
using System.Collections;

public class EnemyBoomerangShooter : MonoBehaviour
{
    public GameObject boomerangPrefab;
    public Transform firePoint;
    public float attackCooldown = 3f;

    private Transform player;
    private bool isAttacking = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // š Update ‚ÍŠ®‘Síœi©—¥AI‹Ö~j
    void Update() { }

    // UtilityAI ‚©‚ç’¼ÚŒÄ‚Ô—Bˆê‚Ì“üŒû
    public void Shoot()
    {
        if (isAttacking) return;

        if (player == null)
        {
            Debug.LogError("[Boomerang] Player not found");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("[Boomerang] firePoint ‚ª null");
            return;
        }

        if (boomerangPrefab == null)
        {
            Debug.LogError("[Boomerang] prefab ‚ª null");
            return;
        }

        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.1f);

        GameObject boomerang = Instantiate(
            boomerangPrefab,
            firePoint.position,
            Quaternion.identity
        );

        BoomerangAttackFromEnemy script =
            boomerang.GetComponent<BoomerangAttackFromEnemy>();

        if (script == null)
        {
            Debug.LogError("BoomerangAttackFromEnemy ‚ª‚ ‚è‚Ü‚¹‚ñ");
            yield break;
        }

        script.Initialize(player, this.transform);

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}
