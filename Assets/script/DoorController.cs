using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform buttonTop; // �{�^���̒��ݍ��݂��Ď����镔��
    public float thresholdY = -20f; // �ǂꂭ�炢���񂾂�h�A���J����
    public Transform door;
    public Vector3 openOffset = new Vector3(0, 3, 0); // �J�����Ƃ��̈ړ���

    private Vector3 originalDoorPos;

    void Start()
    {
        originalDoorPos = door.position;
    }

    void Update()
    {
        if (buttonTop.localPosition.y < thresholdY)
        {
            // �J���i��Ɉړ��j
            door.position = Vector3.Lerp(door.position, originalDoorPos + openOffset, Time.deltaTime * 3f);
        }
        else
        {
            // ����i���ɖ߂�j
            door.position = Vector3.Lerp(door.position, originalDoorPos, Time.deltaTime * 3f);
        }
    }
}
