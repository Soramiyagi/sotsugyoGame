using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public GameObject door;            // �h�A�̃I�u�W�F�N�g
    public float openHeight = 3f;      // �J������
    public float openSpeed = 2f;       // �ړ��X�s�[�h

    private bool isPressed = false;    // ������Ă��邩
    private Vector3 originalPosition;  // �h�A�̌��̈ʒu
    private Vector3 openPosition;      // �h�A���J�����Ƃ��̈ʒu

    void Start()
    {
        // �h�A�̏����ʒu���L�^
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
            // �h�A���J�����ʒu�ւ������ړ�
            door.transform.position = Vector3.MoveTowards(door.transform.position, openPosition, openSpeed * Time.deltaTime);
        }
        else
        {
            // �h�A�����̈ʒu�ɂ������߂�
            door.transform.position = Vector3.MoveTowards(door.transform.position, originalPosition, openSpeed * Time.deltaTime);
        }
    }
}