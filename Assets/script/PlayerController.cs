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

    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
                }
                else
                {
                    animator.SetTrigger("Attack");
                }
            }
            else
            {
                if (verticalInput > 0.1f)
                {
                    animator.SetTrigger("AirAttackUp");
                    Debug.Log("adda");
                }
                else if (verticalInput < -0.1f)
                {
                    animator.SetTrigger("AirAttackDown");
                }
                else
                {
                    animator.SetTrigger("AirAttack");
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
        if (!isGrounded)
        {
            float vy = rb.velocity.y;

            if (vy > 0.1f)
            {
                CurrentState = PlayerState.JumpRise;
            }
            else if (vy < -0.1f)
            {
                CurrentState = PlayerState.JumpFall;
            }
            else
            {
                CurrentState = PlayerState.JumpMid;
            }
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
    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}

