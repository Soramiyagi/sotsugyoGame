using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] bosses;

    private bool isTriggered = false;

    private void Start()
    {
        // 最初は全ボス非表示
        foreach (var boss in bosses)
        {
            boss.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTriggered) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            SpawnBosses();
        }
    }

    private void SpawnBosses()
    {
        foreach (var boss in bosses)
        {
            boss.SetActive(true);
        }
    }
}
