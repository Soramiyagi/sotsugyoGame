using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrolEnemy : EnemyBase
{
    public float moveSpeed = 2f;          // 移動速度
    public float leftLimit = -3f;         // 左端のx座標
    public float rightLimit = 3f;         // 右端のx座標

    private int moveDirection = 1;        // 1 = 右、-1 = 左

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        // 移動
        transform.Translate(Vector2.right * moveSpeed * moveDirection * Time.deltaTime);

        // 端に到達したら方向転換
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

    // 見た目の反転
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * moveDirection;  // 方向に合わせて正負を変える
        transform.localScale = scale;
    }
}



