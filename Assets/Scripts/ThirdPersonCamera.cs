using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 100f;
    public float distance = 5f;
    public float height = 2f;
    
    private float mouseX;
    private float mouseY;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void LateUpdate()
    {
        // Get mouse input
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, -35f, 60f);
        
        // Calculate camera position
        Vector3 targetPosition = player.position - (Quaternion.Euler(mouseY, mouseX, 0) * Vector3.forward * distance);
        targetPosition.y = player.position.y + height;
        
        transform.position = targetPosition;
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}
