using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    public float amplitude = 0.5f;    // �㉺�ړ��̕�
    public float speed = 1.0f;        // �����̑���

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // �����ʒu��ۑ�
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
