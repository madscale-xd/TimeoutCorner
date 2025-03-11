using UnityEngine;
using System.Collections;

public class TriggerActivator : MonoBehaviour
{
    [Header("Target Object & Movement Settings")]
    public GameObject targetObject; // Assign the object that should move
    public float moveSpeed = 7f; // Speed of movement
    public float moveDuration = 2f; // Time the object moves

    private bool isMoving = false;
    private Coroutine moveCoroutine; // Store coroutine reference

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovingPlatform") && !isMoving) // Only activate if the player enters
        {
            moveCoroutine = StartCoroutine(MoveObject());
        }
    }

    private IEnumerator MoveObject()
    {
        if (targetObject == null) yield break; // Don't run if no object is assigned
        
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 moveDirection = targetObject.transform.right; // Move in its right direction

        while (elapsedTime < moveDuration)
        {
            targetObject.transform.position += moveDirection * moveSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        isMoving = false;
    }

    // 🛑 **Call this method to instantly stop movement**
    public void StopMovement()
    {
        if (moveCoroutine != null) 
        {
            StopCoroutine(moveCoroutine); // Stop the movement coroutine
        }
        isMoving = false;
    }
}
