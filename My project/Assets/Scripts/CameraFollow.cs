using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;      // Assign Player in Inspector
    public Vector3 offset;        // Distance between camera and player
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Optional: keep camera looking at player
        // transform.LookAt(player);
    }
}
