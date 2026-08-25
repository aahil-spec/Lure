using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController:MonoBehaviour
{

    Transform t;
    Rigidbody rb;

    [Header("Player Rotation")]
    public float sensitivity=1f;

    //mouse input variables
    float rotationX;
    float rotationY;
    [Header("Land Movement")]
    public float walkSpeed=4f;
    public float sprintSpeed=7f;
    public float jumpForce=6f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundCheckDistance=0.2f;
    bool isGrounded;

    CapsuleCollider capsule;

    void Start()
    {
        t=this.transform;
        rb=GetComponent<Rigidbody>();
        capsule=GetComponent<CapsuleCollider>();

        rb.freezeRotation=true;

        Cursor.lockState=CursorLockMode.Locked;
    }
    
    void Update()
    {
        LookAround();
    
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState=CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }
    void FixedUpdate()
    {
        CheckGrounded();
        MoveLand();
    }

    void LookAround()
    {
        rotationX+=Input.GetAxis("Mouse X")*sensitivity;
        rotationY+=Input.GetAxis("Mouse Y")*sensitivity;

        t.localRotation=Quaternion.Euler(-rotationY,rotationX,0);

    }
    void CheckGrounded()
    {
        Vector3 origin=t.position+Vector3.up*0.1f;
        isGrounded=Physics.Raycast(origin,Vector3.down,groundCheckDistance+0.1f,groundLayer);
    
    }
    void MoveLand()
    {
        float h=Input.GetAxisRaw("Horizontal");
        float v=Input.GetAxisRaw("Vertical");
        float currentSpeed=Input.GetKey(KeyCode.LeftShift)?sprintSpeed:walkSpeed;

        Vector3 moveDir=(t.forward*v+t.right*h).normalized;
        Vector3 targetVelocity=moveDir*currentSpeed;

        Vector3 velocity=rb.linearVelocity;
        velocity.x=targetVelocity.x;
        velocity.z=targetVelocity.z;

        rb.linearVelocity=velocity;
    }
    void Jump()
    {
        Vector3 velocity=rb.linearVelocity;
        velocity.y=jumpForce;
        rb.linearVelocity=velocity;
    }
}
