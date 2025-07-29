using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // プレイヤーのTransform
    public float zOffset = -10f;   // カメラのZ位置（2Dでは-10がデフォルト）
    public float verticalRange = 3f;  // プレイヤーがカメラ中央からどれだけ離れてもよいか

    private float currentY;        // カメラの現在のY位置

    void Start()
    {
        if (player != null)
        {
            currentY = player.position.y;
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            float playerY = player.position.y;

            // プレイヤーが範囲外に出たらカメラYを更新
            if (Mathf.Abs(playerY - currentY) > verticalRange)
            {
                if (playerY > currentY)
                    currentY = playerY - verticalRange;
                else
                    currentY = playerY + verticalRange;
            }

            // カメラ位置を更新
            Vector3 newPos = new Vector3(player.position.x, currentY, zOffset);
            transform.position = newPos;
        }
    }
}

