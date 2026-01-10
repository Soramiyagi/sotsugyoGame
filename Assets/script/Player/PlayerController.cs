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

    public int maxHP = 100;
    private int currentHP;

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
    public float climbSpeed = 3f;

    public Transform respawnPoint;
    private bool isRespawning = false;

    [SerializeField] private FadeScript fadeScript;

    private bool isInvincible;
    [SerializeField] private float invincibleTime = 0.3f;

    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHP = maxHP;

        attackColliderFront = transform.Find("AttackColliderFront")?.GetComponent<Collider2D>();
        attackColliderUp = transform.Find("AttackColliderUp")?.GetComponent<Collider2D>();
        attackColliderDown = transform.Find("AttackColliderDown")?.GetComponent<Collider2D>();

        if (attackColliderFront) attackColliderFront.enabled = false;
        if (attackColliderUp) attackColliderUp.enabled = false;
        if (attackColliderDown) attackColliderDown.enabled = false;
    }

    void Update()
    {
        if (isRespawning) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // 梯子開始
        if (onLadder && !isClimbing && Mathf.Abs(moveY) > 0.1f)
        {
            isClimbing = true;
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }

        if (isClimbing)
        {
            rb.velocity = new Vector2(moveX * moveSpeed, moveY * climbSpeed);

            // ★ ジャンプボタンのみで梯子から離脱
            if (Input.GetButtonDown("Jump"))
            {
                isClimbing = false;
                onLadder = false;
                rb.gravityScale = 1f;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        else
        {
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

            // ★ ジャンプボタンのみ
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }
        }

        if (!onLadder && isClimbing)
        {
            isClimbing = false;
            rb.gravityScale = 1f;
        }

        UpdateState(moveX);
        Flip(moveX);
        UpdateAnimation(moveX, moveY);

        // 攻撃
        if (Input.GetButtonDown("Attack") && canAttack)
        {
            float v = Input.GetAxisRaw("Vertical");

            if (isGrounded || isClimbing)
            {
                if (v > 0.1f)
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
                if (v > 0.1f)
                {
                    animator.SetTrigger("AirAttackUp");
                    StartCoroutine(EnableAttackCollider(attackColliderUp));
                }
                else if (v < -0.1f)
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
            if (vy > 1.5f) CurrentState = PlayerState.JumpRise;
            else if (vy < -1.5f) CurrentState = PlayerState.JumpFall;
            else CurrentState = PlayerState.JumpMid;
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
            bool moving = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveY) > 0.1f;
            animator.SetBool("LadderMove", moving);
            animator.SetBool("LadderIdle", !moving);

            animator.SetBool("Moving", false);
            animator.SetBool("JumpRise", false);
            animator.SetBool("JumpMid", false);
            animator.SetBool("JumpFall", false);
        }
        else
        {
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
        if (moveX > 0 && !facingRight || moveX < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("swap"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("swap"))
            isGrounded = false;
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator EnableAttackCollider(Collider2D col)
    {
        col.enabled = true;
        yield return new WaitForSeconds(0.2f);
        col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            onLadder = true;

        if (collision.CompareTag("Enemy") || collision.CompareTag("Attack") || collision.CompareTag("SwapEnemy"))
            TakeDamage(20);

        if (collision.CompareTag("Checkpoint"))
            respawnPoint = collision.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            onLadder = false;
    }


    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHP -= damage;

        if (currentHP <= 0)
        {
            StartCoroutine(Respawn());
        }
        else
        {
            StartCoroutine(InvincibleRoutine());
        }
    }

    private IEnumerator InvincibleRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    private IEnumerator Respawn()
    {
        isRespawning = true;

        yield return StartCoroutine(fadeScript.FadeOut());

        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;

        animator.SetTrigger("Die");
        yield return new WaitForSeconds(1f);

        transform.position = respawnPoint.position;
        currentHP = maxHP;

        rb.isKinematic = false;
        GetComponent<Collider2D>().enabled = true;

        yield return StartCoroutine(fadeScript.FadeIn());
        isRespawning = false;
    }

    public float PerHP => Mathf.Clamp01((float)currentHP / maxHP);
}
