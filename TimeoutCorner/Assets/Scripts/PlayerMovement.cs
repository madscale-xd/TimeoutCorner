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
    private bool onBoat = false;
    private bool movingSideways = false;
    private Coroutine ungroundCoroutine; // Stores reference to lingering coroutine
    private float lastResetTime;
    private TimerScript timerScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent Rigidbody from rotating
        GameObject timerObject = GameObject.Find("TimerUI");
        if (timerObject != null)
        {
            timerScript = timerObject.GetComponent<TimerScript>();
            lastResetTime = Time.time; // Initialize reset time
        }
    }

    void Update()
    {
        float elapsedTime = Time.time - lastResetTime; 

        if (onBoat)
        {
            if (elapsedTime >= 13f) 
            {
                if (!movingSideways) 
                {
                    movingSideways = true;
                }
                HandleMovementBoat();
            }
            else 
            {
                HandleMovement(); // Normal movement before 13s
            }
        }
        else 
        {
            HandleMovement(); // Normal movement if not on the boat
        }

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

    void HandleMovementBoat()
    {
        float x = Input.GetAxis("Horizontal"); // A & D
        float z = Input.GetAxis("Vertical");   // W & S

        Vector3 moveDirection = transform.right * x + transform.forward * z;
        Vector3 moveVelocity = moveDirection.normalized * moveSpeed;

        Rigidbody boatRb = null;
        BoatScript boatScript = null; 
        if (isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
            {
                boatRb = hit.collider.attachedRigidbody;
                boatScript = boatRb != null ? boatRb.GetComponent<BoatScript>() : null;
            }
        }

        Vector3 inheritedVelocity = Vector3.zero;
        if (boatScript != null)
        {
            float elapsedTime = Time.time - lastResetTime;
            if (elapsedTime >= 13f) 
            {
                inheritedVelocity = boatRb.velocity; 
            }
        }

        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z) + inheritedVelocity;
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

            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                onBoat = true;
            }

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
            onBoat = false;
            movingSideways = false; // Ensure sideways movement is reset when leaving the boat

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void OnTimerResetPlayer()
    {
        lastResetTime = Time.time;
        movingSideways = false; // Reset movement state
        onBoat = false; // Ensure player is not locked in boat mode after reset
    }
}
