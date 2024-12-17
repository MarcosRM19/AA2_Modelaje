using UnityEngine;

public class DrOctopusController : MonoBehaviour
{
    [SerializeField] public float speed = 5f; // Speed of movement
    [SerializeField] public float rotationSpeed = 100f; // Speed of rotation
    [SerializeField] public float jumpForce = 1f;        // The force applied for the jump
    [SerializeField] private float jumpCooldown = 1f;
    private bool canJump = true;

    private Vector3 movement;
    private Rigidbody rb;              // Reference to the Rigidbody


    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from WASD or arrow keys
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // Create movement vector

        movement = (transform.right * moveX + transform.forward * moveZ).normalized;

        // Move the player
        rb.AddForce(movement * speed * Time.deltaTime, ForceMode.Impulse);

        // Rotate left when the left mouse button is clicked
        if (Input.GetMouseButton(0)) // Left Mouse Button
        {
            RotateLeft();
        }

        // Rotate right when the right mouse button is clicked
        if (Input.GetMouseButton(1)) // Right Mouse Button
        {
            RotateRight();
        }

        // Jump when the space bar is pressed and the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            Jump();
        }
    }

    void RotateLeft()
    {
        // Rotate around the Y-axis to the left
        transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
    }

    void RotateRight()
    {
        // Rotate around the Y-axis to the right
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
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
}