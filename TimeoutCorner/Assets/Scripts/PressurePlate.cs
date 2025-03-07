using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Transform targetObject; // Object to move
    [SerializeField] private Vector3 moveOffset = new Vector3(0, -1, 0); // Movement direction & distance
    [SerializeField] private float moveDuration = 2f; // Time in seconds to complete movement

    private Vector3 initialPosition;
    private bool isMoving = false;

    void Start()
    {
        if (targetObject != null)
        {
            initialPosition = targetObject.position; // Store original position
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Switch" && !isMoving)
        {
            StartCoroutine(MoveTargetObject(initialPosition + moveOffset)); // Move slowly
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Switch" && !isMoving)
        {
            StartCoroutine(MoveTargetObject(initialPosition)); // Move back slowly
        }
    }

    IEnumerator MoveTargetObject(Vector3 targetPosition)
    {
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 startPosition = targetObject.position;

        while (elapsedTime < moveDuration)
        {
            targetObject.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        targetObject.position = targetPosition; // Ensure it reaches exact position
        isMoving = false;
    }
}
