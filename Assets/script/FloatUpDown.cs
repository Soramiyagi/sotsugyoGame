using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    public float amplitude = 0.5f;    // 上下移動の幅
    public float speed = 1.0f;        // 動きの速さ

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // 初期位置を保存
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
