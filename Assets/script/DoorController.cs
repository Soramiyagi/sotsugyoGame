using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform buttonTop; // ボタンの沈み込みを監視する部分
    public float thresholdY = -20f; // どれくらい沈んだらドアが開くか
    public Transform door;
    public Vector3 openOffset = new Vector3(0, 3, 0); // 開いたときの移動量

    private Vector3 originalDoorPos;

    void Start()
    {
        originalDoorPos = door.position;
    }

    void Update()
    {
        if (buttonTop.localPosition.y < thresholdY)
        {
            // 開く（上に移動）
            door.position = Vector3.Lerp(door.position, originalDoorPos + openOffset, Time.deltaTime * 3f);
        }
        else
        {
            // 閉じる（元に戻る）
            door.position = Vector3.Lerp(door.position, originalDoorPos, Time.deltaTime * 3f);
        }
    }
}
