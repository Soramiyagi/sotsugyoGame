using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrolEnemy : EnemyBase
{
    public float moveSpeed = 2f;          // �ړ����x
    public float leftLimit = -3f;         // ���[��x���W
    public float rightLimit = 3f;         // �E�[��x���W

    private int moveDirection = 1;        // 1 = �E�A-1 = ��

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        // �ړ�
        transform.Translate(Vector2.right * moveSpeed * moveDirection * Time.deltaTime);

        // �[�ɓ��B����������]��
        if (transform.position.x > rightLimit)
        {
            moveDirection = -1;
            Flip();
        }
        else if (transform.position.x < leftLimit)
        {
            moveDirection = 1;
            Flip();
        }
    }

    // �����ڂ̔��]
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * moveDirection;  // �����ɍ��킹�Đ�����ς���
        transform.localScale = scale;
    }
}



