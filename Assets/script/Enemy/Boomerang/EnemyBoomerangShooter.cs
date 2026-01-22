using UnityEngine;
using System.Collections;

public class EnemyBoomerangShooter : MonoBehaviour
{
    public GameObject boomerangPrefab;
    public Transform firePoint;
    public float attackCooldown = 3f;

    public float delayAnime = 0.1f;

    private Transform player;
    private bool isAttacking = false;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponentInChildren<Animator>();
    }

    void Update() { } // ©—¥AI‹Ö~

    // UtilityAI ‚©‚çŒÄ‚Ô—Bˆê‚Ì“üŒû
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

        // š ‚±‚±‚ÅƒAƒjƒƒgƒŠƒK[”­‰Î
        if (animator != null)
            animator.SetTrigger("IsBoomerang");

        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        isAttacking = true;
       
        // ƒAƒjƒ‚É‡‚í‚¹‚Ä­‚µ’x‰„
        yield return new WaitForSeconds(delayAnime);

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
