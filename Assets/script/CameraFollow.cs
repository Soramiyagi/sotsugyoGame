using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // �v���C���[��Transform
    public float zOffset = -10f;   // �J������Z�ʒu�i2D�ł�-10���f�t�H���g�j
    public float verticalRange = 3f;  // �v���C���[���J������������ǂꂾ������Ă��悢��

    private float currentY;        // �J�����̌��݂�Y�ʒu

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

            // �v���C���[���͈͊O�ɏo����J����Y���X�V
            if (Mathf.Abs(playerY - currentY) > verticalRange)
            {
                if (playerY > currentY)
                    currentY = playerY - verticalRange;
                else
                    currentY = playerY + verticalRange;
            }

            // �J�����ʒu���X�V
            Vector3 newPos = new Vector3(player.position.x, currentY, zOffset);
            transform.position = newPos;
        }
    }
}

