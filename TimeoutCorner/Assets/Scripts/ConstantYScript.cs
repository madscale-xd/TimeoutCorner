using UnityEngine;

public class ConstantYMovement : MonoBehaviour
{
    public float speed = 2f; // Adjust this to change the movement speed

    void Update()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }
}
