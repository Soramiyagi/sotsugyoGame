using UnityEngine;

public class EnemyContext : MonoBehaviour
{
    public Transform player;
    public float health = 100f;
    public float maxHealth = 100f;
    public bool canSeePlayer = false;
    [HideInInspector] public float lastDamageTime = -999f;

    // ���x��˒��ȂǁA�s���ŎQ�Ƃ��������������ɒǉ�
    public float timeSinceLastAttack => Time.time - lastDamageTime;

    void Update()
    {
        // �ȈՓI�Ȏ��F����i��j - raycast�Ŏ��F���Z�b�g�������Ȃ炱���ł��Ηǂ�
        if (player != null)
        {
            Vector3 dir = player.position - transform.position;
            canSeePlayer = Vector3.Angle(transform.forward, dir) < 60f && dir.magnitude < 20f;
        }
    }
}

