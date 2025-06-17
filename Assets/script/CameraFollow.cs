using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // �v���C���[��Transform
    public float fixedY = 0f;      // �J������Y�ʒu�i�Œ�l�j
    public float zOffset = -10f;   // �J������Z�ʒu�i2D�ł�-10���f�t�H���g�j

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 newPos = new Vector3(player.position.x, fixedY, zOffset);
            transform.position = newPos;
        }
    }
}
