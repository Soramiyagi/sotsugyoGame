using UnityEngine;

public class SwapObject : MonoBehaviour
{
    public float radius = 1.0f;
    private bool swapReady = false;
    private Vector3 swapObjPos;                 //����ւ��Ώۂ̈ʒu
    private GameObject nearestSwap = null;      //����ւ��Ώۂ��̂���
    public Vector3 offset;

    void Update()
    {
        // F�L�[��swap�I�u�W�F�N�g�̈ʒu��ۑ�
        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector3 center = transform.position + offset;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);

            nearestSwap = null;
            float nearestDistance = float.MaxValue;

            foreach (Collider2D collider in hitColliders)
            {
                if (collider.CompareTag("swap")|| collider.CompareTag("SwapEnemy"))
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
                swapObjPos = nearestSwap.transform.position;
                swapReady = true;
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
            if (swapReady)
            {
                Vector3 temp = transform.position + offset;
                transform.position = swapObjPos;
                nearestSwap.transform.position = temp;

                Debug.Log("�v���C���[��swap�I�u�W�F�N�g�̈ʒu�����ւ��܂����B");

                // �ۑ���j��
                nearestSwap = null;
                swapReady = false;
            }
            else
            {
                Debug.Log("�ۑ����ꂽswap�I�u�W�F�N�g������܂���BF�L�[�Ő�ɕۑ����Ă��������B");
            }
        }
    }

    // �����i�V�[���r���[�p�j
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + offset;
        Gizmos.DrawWireSphere(center, radius);
    }
}



