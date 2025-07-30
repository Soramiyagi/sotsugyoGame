using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn respawnManager = FindObjectOfType<PlayerRespawn>();
            if (respawnManager != null)
            {
                respawnManager.UpdateCheckpoint(transform);
            }
        }
    }
}

