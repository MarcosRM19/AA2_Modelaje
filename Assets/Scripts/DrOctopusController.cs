using UnityEngine;

public class DrOctopusController : MonoBehaviour
{
    [SerializeField] public float speed = 5f; // Speed of movement
    [SerializeField] public float rotationSpeed = 100f; // Speed of rotation
    [SerializeField] public float jumpForce = 1f;        // The force applied for the jump
    [SerializeField] private float jumpCooldown = 1f;
    [SerializeField] private TakeTarget[] takeSpiderman;
    [SerializeField] private float mouseSensitivity = 100f; // Rotation sensitivity
    private bool canJump = true;

    private Vector3 movement;
    private Rigidbody rb;              // Reference to the Rigidbody

    private float xRotation = 0f; // Vertical rotation
    private Vector3 dragOrigin; // Dragging reference point

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        // Jump when the space bar is pressed and the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
        }

        HandleMouseLook();
    }

    void Move()
    {
        // Get input from WASD or arrow keys
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // Create movement vector

        movement = (transform.right * moveX + transform.forward * moveZ).normalized;

        // Move the player
        rb.AddForce(movement * speed * Time.deltaTime, ForceMode.Impulse);
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // Apply upward force
        canJump = false;
        Invoke("OctaviusCanJump", jumpCooldown);
    }

    void OctaviusCanJump()
    {
        canJump = true;
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp vertical rotation

        transform.localRotation = Quaternion.Euler(xRotation, transform.localRotation.eulerAngles.y + mouseX, 0f);
    }
}