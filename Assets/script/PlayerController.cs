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

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool facingRight = true;
    private Animator animator;
    private bool canAttack = true;
    public float attackCooldown = 0.5f;

    private Collider2D attackColliderFront;
    private Collider2D attackColliderUp;
    private Collider2D attackColliderDown;

    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        attackColliderFront = transform.Find("AttackColliderFront")?.GetComponent<Collider2D>();
        attackColliderUp = transform.Find("AttackColliderUp")?.GetComponent<Collider2D>();
        attackColliderDown = transform.Find("AttackColliderDown")?.GetComponent<Collider2D>();

        if (attackColliderFront == null) Debug.LogError("AttackColliderFront not found!");
        if (attackColliderUp == null) Debug.LogError("AttackColliderUp not found!");
        if (attackColliderDown == null) Debug.LogError("AttackColliderDown not found!");

        // 最初は全て無効化
        if (attackColliderFront) attackColliderFront.enabled = false;
        if (attackColliderUp) attackColliderUp.enabled = false;
        if (attackColliderDown) attackColliderDown.enabled = false;


    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
        if (Input.GetMouseButtonDown(1) && canAttack)
        {
            float verticalInput = Input.GetAxisRaw("Vertical");

            if (isGrounded)
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
            else
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

        UpdateState(moveX);
        Flip(moveX);
        UpdateAnimation();
    }

    void UpdateState(float moveX)
    {
        Debug.Log(isGrounded);
        if (!isGrounded)
        {
            float vy = rb.velocity.y;

            if (vy > 0.1f)
            {
                //Debug.Log("Rise");
                CurrentState = PlayerState.JumpRise;
            }
            else if (vy < -0.1f)
            {
                //Debug.Log("Fall");
                CurrentState = PlayerState.JumpFall;
            }
            else
            {
                //Debug.Log("Mid");
                CurrentState = PlayerState.JumpMid;
            }
        }
        else if (Mathf.Abs(moveX) > 0.01f)
        {
            //Debug.Log("Move");
            CurrentState = PlayerState.Moving;
        }
        else
        {
            //Debug.Log("Idle");
            CurrentState = PlayerState.Idle;
        }
    }

    void UpdateAnimation()
    {
        animator.SetBool("Moving", CurrentState == PlayerState.Moving);
        animator.SetBool("JumpRise", CurrentState == PlayerState.JumpRise);
        animator.SetBool("JumpMid", CurrentState == PlayerState.JumpMid);
        animator.SetBool("JumpFall", CurrentState == PlayerState.JumpFall);
        // Idleアニメーションは全てのジャンプがfalse、Movingがfalseのとき再生
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
        yield return new WaitForSeconds(0.2f); // 当たり判定を出す時間（必要に応じて調整）
        collider.enabled = false;
    }

}

