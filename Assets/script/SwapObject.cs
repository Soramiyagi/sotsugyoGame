using UnityEngine;

public class SwapObjectFinder2D : MonoBehaviour
{
    public float radius = 1.0f;
    private Transform savedSwapTransform = null;

    void Update()
    {
        // F�L�[��swap�I�u�W�F�N�g�̈ʒu��ۑ�
        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector2 center = (Vector2)transform.position + Vector2.up;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);

            GameObject nearestSwap = null;
            float nearestDistance = float.MaxValue;

            foreach (Collider2D collider in hitColliders)
            {
                if (collider.CompareTag("swap"))
                {
                    float distance = Vector2.Distance(transform.position, collider.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestSwap = collider.gameObject;
                    }
                }
            }

            if (nearestSwap != null)
            {
                savedSwapTransform = nearestSwap.transform;
                Debug.Log("�ۑ����ꂽswap�I�u�W�F�N�g: " + nearestSwap.name);
            }
            else
            {
                Debug.Log("swap�^�O�̃I�u�W�F�N�g��������܂���ł����B");
            }
        }

        // E�L�[�ňʒu�����ւ� & �ۑ��j��
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (savedSwapTransform != null)
            {
                Vector3 temp = transform.position;
                transform.position = savedSwapTransform.position;
                savedSwapTransform.position = temp;

                Debug.Log("�v���C���[��swap�I�u�W�F�N�g�̈ʒu�����ւ��܂����B");

                // �ۑ���j��
                savedSwapTransform = null;
            }
            else
            {
                Debug.Log("�ۑ����ꂽswap�I�u�W�F�N�g������܂���BF�L�[�Ő�ɕۑ����Ă��������B");
            }
        }
    }

    // �����i�V�[���r���[�p�j
    private void OnDrawGizmosSelected()
    {
        Vector2 center = (Vector2)transform.position + Vector2.up;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, radius);
    }
}



