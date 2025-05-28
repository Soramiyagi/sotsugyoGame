using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    public float amplitude = 1.0f; // ���E�̈ړ����i�ő�}1.0�j
    public float speed = 1.0f;     // �ړ��X�s�[�h

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // �����ʒu��ۑ�
    }

    void Update()
    {
        float newX = startPos.x + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(newX, startPos.y, startPos.z);
    }
}
