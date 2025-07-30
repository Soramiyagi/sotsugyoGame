using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    public static PlayerRespawn Instance;

    public GameObject playerPrefab;
    public Transform initialSpawnPoint;        // 初期スポーン位置（インスペクターから設定）

    private Transform currentCheckpoint;       // 現在のチェックポイント

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {

        currentCheckpoint = initialSpawnPoint;

        // ゲーム開始時にプレイヤーを出現
        SpawnPlayer();
    }

    public void UpdateCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    public void RespawnPlayer()
    {
        Invoke(nameof(SpawnPlayer), 1f); // 1秒後にリスポーン
    }

    private void SpawnPlayer()
    {
        Instantiate(playerPrefab, currentCheckpoint.position, Quaternion.identity);
    }
}
