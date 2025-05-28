using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    public float amplitude = 0.5f;    // ã‰ºˆÚ“®‚Ì•
    public float speed = 1.0f;        // “®‚«‚Ì‘¬‚³

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // ‰ŠúˆÊ’u‚ğ•Û‘¶
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
