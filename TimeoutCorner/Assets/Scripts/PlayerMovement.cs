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
    private bool wasGrounded;
    private bool onBoat = false;
    private bool movingSideways = false;

    private bool isMoving = false;
    private Coroutine ungroundCoroutine; // Stores reference to lingering coroutine
    private float lastResetTime;
    private TimerScript timerScript;
    private SFXManager sfx;
    public Transform teleportTarget; // Assign this in the inspector

    private Vector3 lastVelocity; // Track velocity to check falling
    [SerializeField] private float minFallSpeed = -3f; // Minimum speed before landing SFX triggers
    [SerializeField] private Transform headTransform; // Assign the player's camera or head transform
    [SerializeField] private float headBobAmount = 0.1f; // How much the head moves
    [SerializeField] private float headBobDuration = 0.15f; // How long the effect lasts
    private Vector3 originalHeadPosition;
    private bool isHeadBobbing = false; // Prevents overlapping head bob effects

    void Start()
    {
        originalHeadPosition = headTransform.localPosition;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent Rigidbody from rotating
        GameObject timerObject = GameObject.Find("TimerUI");
        if (timerObject != null)
        {
            timerScript = timerObject.GetComponent<TimerScript>();
            sfx = timerObject.GetComponent<SFXManager>();
            lastResetTime = Time.time; // Initialize reset time
        }
    }

    void Update()
    {
        lastVelocity = rb.velocity; // Store velocity every frame
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

        bool wasMoving = isMoving; // Store previous state
        bool previouslyGrounded = wasGrounded;

        isMoving = (x != 0 || z != 0); // Check input
        wasGrounded = isGrounded; // Ensure `isGrounded` updates properly

        HandleMovementSFX(wasMoving, previouslyGrounded);
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

        bool wasMoving = isMoving; // Store previous state
        bool previouslyGrounded = wasGrounded;

        isMoving = (x != 0 || z != 0); // Check input
        wasGrounded = isGrounded; // Ensure `isGrounded` updates properly

        HandleMovementSFX(wasMoving, previouslyGrounded);
    }

    void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            sfx.PlaySFX(1); 
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("MovingPlatform"))
        {
            // Only play SFX and bob if falling fast enough
            if (lastVelocity.y < minFallSpeed && !isHeadBobbing) 
            {
                sfx.PlaySFX(2); // Play landing SFX
                StartCoroutine(SmoothHeadBob()); // Start smooth head bob effect
            }
        }
    }

    private IEnumerator SmoothHeadBob()
    {
        isHeadBobbing = true; 
        float elapsedTime = 0f;

        // Move head down smoothly
        while (elapsedTime < headBobDuration / 2)
        {
            headTransform.localPosition = Vector3.Lerp(originalHeadPosition, 
                                                    originalHeadPosition + Vector3.down * headBobAmount, 
                                                    elapsedTime / (headBobDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        // Move head back up smoothly
        while (elapsedTime < headBobDuration / 2)
        {
            headTransform.localPosition = Vector3.Lerp(originalHeadPosition + Vector3.down * headBobAmount, 
                                                    originalHeadPosition, 
                                                    elapsedTime / (headBobDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        headTransform.localPosition = originalHeadPosition; // Ensure it resets fully
        isHeadBobbing = false;
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
        yield return new WaitForSeconds(0.01f);
        isGrounded = false;
        ungroundCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (other.CompareTag("WardenCube") || other.CompareTag("WardenPrime") && teleportTarget != null)
        {
            transform.position = teleportTarget.position;
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

    private void HandleMovementSFX(bool wasMoving, bool wasGrounded)
{
    if (sfx == null) return; // Safety check

    // Start playing when movement starts & player is grounded
    if (isMoving && !wasMoving && isGrounded) 
    {
        sfx.LoopSFX(0); 
    }
    // Stop playing if player stops moving OR becomes airborne
    else if ((!isMoving || !isGrounded) && wasMoving) 
    {
        sfx.StopSFX(0);
    }
    // Resume playing if player was airborne and just landed while moving
    else if (isMoving && isGrounded && !wasGrounded) 
    {
        sfx.LoopSFX(0);
    }
}
}
