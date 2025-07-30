using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    public static PlayerRespawn Instance;

    public GameObject playerPrefab;
    public Transform initialSpawnPoint;        // �����X�|�[���ʒu�i�C���X�y�N�^�[����ݒ�j

    private Transform currentCheckpoint;       // ���݂̃`�F�b�N�|�C���g

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {

        currentCheckpoint = initialSpawnPoint;

        // �Q�[���J�n���Ƀv���C���[���o��
        SpawnPlayer();
    }

    public void UpdateCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    public void RespawnPlayer()
    {
        Invoke(nameof(SpawnPlayer), 1f); // 1�b��Ƀ��X�|�[��
    }

    private void SpawnPlayer()
    {
        Instantiate(playerPrefab, currentCheckpoint.position, Quaternion.identity);
    }
}
