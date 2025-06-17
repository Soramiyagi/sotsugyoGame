using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // プレイヤーのTransform
    public float fixedY = 0f;      // カメラのY位置（固定値）
    public float zOffset = -10f;   // カメラのZ位置（2Dでは-10がデフォルト）

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 newPos = new Vector3(player.position.x, fixedY, zOffset);
            transform.position = newPos;
        }
    }
}
