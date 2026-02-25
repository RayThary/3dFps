using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float runSpeed = 7f;

    [Header("Jump & Gravity")]
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 2f;

    [Header("Mouse")]
    [SerializeField] float mouseSpeed = 1.5f;

    [Header("Camera Pivot (Pitch¿ë)")]
    [SerializeField] Transform cameraPivot;

    float xRot;
    Vector3 velocity;

    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Look();
        MoveAndJump();
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed;

        // Pitch
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        cameraPivot.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        // Yaw
        transform.Rotate(Vector3.up * mouseX);
    }

    void MoveAndJump()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool grounded = cc.isGrounded;

        if (grounded && velocity.y < 0f)
            velocity.y = -2f;

        float curSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 moveDir = transform.right * h + transform.forward * v;
        moveDir = moveDir.normalized;

        cc.Move(moveDir * curSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }
}
