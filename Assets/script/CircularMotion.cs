using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    public float radius = 1.0f;     // �~�̔��a
    public float speed = 1.0f;      // ��]���x�i�傫���قǑ����j

    private Vector3 center;

    void Start()
    {
        center = transform.position; // ���S�ʒu��ۑ�
    }

    void Update()
    {
        float angle = Time.time * speed;
        float x = center.x + Mathf.Cos(angle) * radius;
        float y = center.y + Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
