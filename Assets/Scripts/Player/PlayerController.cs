using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]

public class PlayerController : MonoBehaviour
{
    #region Tuneable Parameters
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public int maxJumps = 2;

    private int jumpsRemaining;
    #endregion

    #region Ground Check Parameters
    public Transform groundCheckPos;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;

    private Vector2 CalculateGroundCheckPos()
    {
        Bounds bounds = bc.bounds;
        return new Vector2(bounds.center.x, bounds.min.y);
    }
    #endregion

    #region Components References
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private Animator anim;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        // Initialize available jumps
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        if (rb.linearVelocityY <= 0f)
        {
            isGrounded = Physics2D.OverlapCircle(
                CalculateGroundCheckPos(),
                groundCheckRadius,
                groundLayer
            );

            // Reset jumps when landing
            if (isGrounded)
            {
                jumpsRemaining = maxJumps;
            }
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        bool jumpPressed = Input.GetButtonDown("Jump");

        rb.linearVelocityX = moveX * moveSpeed;


        if (jumpPressed && jumpsRemaining > 0)
        {
            rb.linearVelocityY = 0f;

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            jumpsRemaining--;

            isGrounded = false;
        }

        if (moveX != 0f)
        {
            SpriteFlip(moveX);
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("IsAttack");
        }

        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetTrigger("IsjumpAttack");
        }

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("moveX", Mathf.Abs(moveX));


    }

    void SpriteFlip(float moveX) => sr.flipX = (moveX < 0f);

    public void UnlockGrapple()
    {
        GetComponent<GrappleHook>().enabled = true;
        GrappleHook grapple = GetComponent<GrappleHook>();

        if (grapple != null)
        {
            grapple.UnlockGrapple();
        }
    }
}