using UnityEngine;

public class EnemyContext : MonoBehaviour
{
    public Transform player;
    public float health = 100f;
    public float maxHealth = 100f;
    public bool canSeePlayer = false;
    [HideInInspector] public float lastDamageTime = -999f;

    // 速度や射程など、行動で参照したい情報をここに追加
    public float timeSinceLastAttack => Time.time - lastDamageTime;

    void Update()
    {
        // 簡易的な視認判定（例） - raycastで視認をセットしたいならここでやれば良い
        if (player != null)
        {
            Vector3 dir = player.position - transform.position;
            canSeePlayer = Vector3.Angle(transform.forward, dir) < 60f && dir.magnitude < 20f;
        }
    }
}

