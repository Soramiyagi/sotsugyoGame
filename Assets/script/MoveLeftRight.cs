using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    public float amplitude = 1.0f; // 左右の移動幅（最大±1.0）
    public float speed = 1.0f;     // 移動スピード

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // 初期位置を保存
    }

    void Update()
    {
        float newX = startPos.x + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(newX, startPos.y, startPos.z);
    }
}
