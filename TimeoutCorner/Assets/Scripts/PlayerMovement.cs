using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private float extraGravity = 2f; // Adjust for faster fall speed

    private Rigidbody rb;
    private bool isGrounded;
    private Coroutine ungroundCoroutine; // Stores reference to lingering coroutine

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent Rigidbody from rotating
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    void FixedUpdate()
    {
        if (!isGrounded) // Apply only when in the air
        {
            rb.velocity += Vector3.down * extraGravity * Time.fixedDeltaTime;
        }
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal"); // A & D
        float z = Input.GetAxis("Vertical");   // W & S

        Vector3 moveDirection = transform.right * x + transform.forward * z;
        Vector3 moveVelocity = moveDirection.normalized * moveSpeed;

        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    // ✅ Collision-based ground detection (for objects tagged "Ground" or "MovingPlatform")
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("MovingPlatform"))
        {
            isGrounded = true;

            // Stop any lingering coroutine (to prevent overriding)
            if (ungroundCoroutine != null)
            {
                StopCoroutine(ungroundCoroutine);
                ungroundCoroutine = null;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        else if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            // Start lingering effect
            if (ungroundCoroutine == null)
            {
                ungroundCoroutine = StartCoroutine(LingerGroundCheck());
            }
        }
    }

    // ⏳ Linger ground check for 0.5s before setting isGrounded to false
    private IEnumerator LingerGroundCheck()
    {
        yield return new WaitForSeconds(0.5f);
        isGrounded = false;
        ungroundCoroutine = null;
    }
}
