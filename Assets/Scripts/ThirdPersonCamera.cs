using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;

    [Header("Camera")]
    public float distance = 6f;
    public float height = 3f;
    public float followSpeed = 10f;

    [Header("Keyboard Camera")]
    public float rotateSpeed = 100f;

    private float yaw;
    private float pitch = 20f;
    private Vector3 cameraVelocity;

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Q and E rotate the camera
        float rotateInput = 0f;

        if (Keyboard.current.qKey.isPressed)
        {
            rotateInput = -1f;
        }

        if (Keyboard.current.eKey.isPressed)
        {
            rotateInput = 1f;
        }

        yaw += rotateInput * rotateSpeed * Time.deltaTime;

        // Keep the camera angle within a reasonable range
        pitch = Mathf.Clamp(pitch, -10f, 70f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 offset =
            rotation * new Vector3(0f, 0f, -distance);

        Vector3 targetPosition =
            target.position +
            Vector3.up * height +
            offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref cameraVelocity,
            0.05f
        );

        transform.LookAt(
            target.position + Vector3.up * 0.5f
        );
    }
}