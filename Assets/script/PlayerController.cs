using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // �ړ����x
    public float jumpForce = 7f; // �W�����v��

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ���������̓��͎擾 (A/D�L�[�܂��́�/���L�[)
        float moveX = Input.GetAxis("Horizontal");

        // ���E�Ɉړ�
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // �W�����v����
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // �n�ʂɐG��Ă��邩�ǂ����𔻒�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
