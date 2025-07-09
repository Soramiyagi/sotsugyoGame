using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public GameObject door;            // ドアのオブジェクト
    public float openHeight = 3f;      // 開く高さ
    public float openSpeed = 2f;       // 移動スピード

    private bool isPressed = false;    // 押されているか
    private Vector3 originalPosition;  // ドアの元の位置
    private Vector3 openPosition;      // ドアが開いたときの位置

    void Start()
    {
        // ドアの初期位置を記録
        originalPosition = door.transform.position;
        openPosition = originalPosition + new Vector3(0f, openHeight, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("swap"))
        {
            isPressed = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("swap"))
        {
            isPressed = false;
        }
    }

    void Update()
    {
        if (isPressed)
        {
            // ドアを開いた位置へゆっくり移動
            door.transform.position = Vector3.MoveTowards(door.transform.position, openPosition, openSpeed * Time.deltaTime);
        }
        else
        {
            // ドアを元の位置にゆっくり戻す
            door.transform.position = Vector3.MoveTowards(door.transform.position, originalPosition, openSpeed * Time.deltaTime);
        }
    }
}