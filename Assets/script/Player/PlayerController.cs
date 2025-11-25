using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Moving,
        JumpRise,
        JumpMid,
        JumpFall
    }

    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public int maxHP = 100; // ★追加：最大HP
    private int currentHP;   // ★追加：現在のHP

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;
    private Animator animator;
    private bool canAttack = true;
    public float attackCooldown = 0.5f;

    private Collider2D attackColliderFront;
    private Collider2D attackColliderUp;
    private Collider2D attackColliderDown;

    private bool isClimbing = false;
    private bool onLadder = false;
    public float climbSpeed = 3f; // ★梯子移動のスピード


    //リスポーン関連
    public Transform respawnPoint; // インスペクターで設定
    private bool isRespawning = false;

    //フェードイン演出
    [SerializeField] private FadeScript fadeScript;


    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHP = maxHP; // ★追加：開始時にHPを満タンに

        attackColliderFront = transform.Find("AttackColliderFront")?.GetComponent<Collider2D>();
        attackColliderUp = transform.Find("AttackColliderUp")?.GetComponent<Collider2D>();
        attackColliderDown = transform.Find("AttackColliderDown")?.GetComponent<Collider2D>();

        if (attackColliderFront == null) Debug.LogError("AttackColliderFront not found!");
        if (attackColliderUp == null) Debug.LogError("AttackColliderUp not found!");
        if (attackColliderDown == null) Debug.LogError("AttackColliderDown not found!");

        if (attackColliderFront) attackColliderFront.enabled = false;
        if (attackColliderUp) attackColliderUp.enabled = false;
        if (attackColliderDown) attackColliderDown.enabled = false;

       
    }

    void Update()
    {

        if (isRespawning) return;
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // 梯子に入っていて、↑または↓が押されたときだけClimb開始
        if (onLadder && !isClimbing && Mathf.Abs(moveY) > 0.1f)
        {
            isClimbing = true;
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero; // 初期化
        }

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(moveX * moveSpeed, moveY * climbSpeed);

            // ジャンプで梯子から離れる
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isClimbing = false;
                onLadder = false;
                rb.gravityScale = 1f;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        else
        {
            // 通常移動
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }
        }

        // 梯子から出たとき（OnTriggerExit）でも保険的に解除
        if (!onLadder && isClimbing)
        {
            isClimbing = false;
            rb.gravityScale = 1f;
        }

        UpdateState(moveX);
        Flip(moveX);
        UpdateAnimation(moveX, moveY);

        if (Input.GetMouseButtonDown(1) && canAttack)
        {
            float verticalInput = Input.GetAxisRaw("Vertical");

            if (isGrounded || isClimbing) // 地上または梯子にいるとき
            {
                if (verticalInput > 0.1f)
                {
                    animator.SetTrigger("AttackUp");
                    StartCoroutine(EnableAttackCollider(attackColliderUp));
                }
                else
                {
                    animator.SetTrigger("Attack");
                    StartCoroutine(EnableAttackCollider(attackColliderFront));
                }
            }
            else // 空中攻撃
            {
                if (verticalInput > 0.1f)
                {
                    animator.SetTrigger("AirAttackUp");
                    StartCoroutine(EnableAttackCollider(attackColliderUp));
                }
                else if (verticalInput < -0.1f)
                {
                    animator.SetTrigger("AirAttackDown");
                    StartCoroutine(EnableAttackCollider(attackColliderDown));
                }
                else
                {
                    animator.SetTrigger("AirAttack");
                    StartCoroutine(EnableAttackCollider(attackColliderFront));
                }
            }

            StartCoroutine(AttackCooldown());
        }
    }



    void UpdateState(float moveX)
    {
        if (!isGrounded)
        {
            float vy = rb.velocity.y;

            if (vy > 1.5f)
                CurrentState = PlayerState.JumpRise;
            else if (vy < -1.5f)
                CurrentState = PlayerState.JumpFall;
            else
                CurrentState = PlayerState.JumpMid;
        }
        else if (Mathf.Abs(moveX) > 0.01f)
        {
            CurrentState = PlayerState.Moving;
        }
        else
        {
            CurrentState = PlayerState.Idle;
        }
    }

    void UpdateAnimation(float moveX, float moveY)
    {
        if (isClimbing)
        {
            // 梯子アニメーションだけON
            bool isClimbMoving = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveY) > 0.1f;
            animator.SetBool("LadderMove", isClimbMoving);
            animator.SetBool("LadderIdle", !isClimbMoving);

            // ジャンプ・移動アニメーションはOFF
            animator.SetBool("Moving", false);
            animator.SetBool("JumpRise", false);
            animator.SetBool("JumpMid", false);
            animator.SetBool("JumpFall", false);
        }
        else
        {
            // 通常アニメーション
            animator.SetBool("LadderMove", false);
            animator.SetBool("LadderIdle", false);

            animator.SetBool("Moving", CurrentState == PlayerState.Moving);
            animator.SetBool("JumpRise", CurrentState == PlayerState.JumpRise);
            animator.SetBool("JumpMid", CurrentState == PlayerState.JumpMid);
            animator.SetBool("JumpFall", CurrentState == PlayerState.JumpFall);
        }
    }

    void Flip(float moveX)
    {
        if (moveX > 0 && !facingRight)
        {
            facingRight = true;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        else if (moveX < 0 && facingRight)
        {
            facingRight = false;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Ground")) || (collision.gameObject.CompareTag("swap")))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("swap")))
        {
            isGrounded = false;
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator EnableAttackCollider(Collider2D collider)
    {
        collider.enabled = true;
        yield return new WaitForSeconds(0.2f);
        collider.enabled = false;
    }

    // ★追加：当たり判定でHPを減らす
    // ★Ladderの当たり判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            onLadder = true;
        }

        if (collision.CompareTag("Attack") || collision.CompareTag("SwapEnemy"))
        {
            TakeDamage(20);
        }
        if (collision.CompareTag("Checkpoint"))
        {
            respawnPoint = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            onLadder = false;
        }
    }

    // ★追加：ダメージ処理
    private void TakeDamage(int damage)
    {
        if (isRespawning) return;

        currentHP -= damage;
        Debug.Log("Player took damage! HP: " + currentHP);

        if (currentHP <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    //リスポーン処理
    private IEnumerator Respawn()
    {
        isRespawning = true;

        yield return StartCoroutine(fadeScript.FadeOut());
        // 操作を無効に（速度ゼロ、重力無効なども必要なら追加）
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;

        animator.SetTrigger("Die"); // 死亡アニメ再生したい場合

        yield return new WaitForSeconds(1f); // 1秒待機

        // 復帰処理
        transform.position = respawnPoint.position;
        currentHP = maxHP;

        rb.isKinematic = false;
        GetComponent<Collider2D>().enabled = true;
        yield return StartCoroutine(fadeScript.FadeIn());
        isRespawning = false;
    }

    public float PerHP
    {
        get
        {
            float value = (float)currentHP / (float)maxHP;
            return Mathf.Clamp(value, 0, 1);
        }
        private set { }
    }

}

