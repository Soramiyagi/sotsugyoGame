using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    public float radius = 1.0f;     // 円の半径
    public float speed = 1.0f;      // 回転速度（大きいほど速い）

    private Vector3 center;

    void Start()
    {
        center = transform.position; // 中心位置を保存
    }

    void Update()
    {
        float angle = Time.time * speed;
        float x = center.x + Mathf.Cos(angle) * radius;
        float y = center.y + Mathf.Sin(angle) * radius;

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
