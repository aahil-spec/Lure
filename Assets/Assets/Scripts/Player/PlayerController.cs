using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController:MonoBehaviour
{

    Transform t;
    Rigidbody rb;
    public Animator anim;

    [Header("Land Movement")]
    public float walkSpeed=4f;
    public float sprintSpeed=7f;
    public float jumpForce=6f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundCheckDistance=0.2f;
    bool isGrounded;

    CapsuleCollider capsule;

    [Header("Water Detection")]
    public LayerMask waterLayer;
    public float waterCheckDistance=50f;

    [Header("Buoyancy")]
    public float buoyancyStrength=15f;
    public float maxBuoyancyForce=20f;
    public float waterDrag=2f;

    public bool isInWater;
    float submersionDepth;
    
    [Header("Underwater Visuals")]
    public Volume underwaterPostProcessing;
    public float transitionSpeed=5f;


    void Start()
    {
        t=this.transform;
        rb=GetComponent<Rigidbody>();
        capsule=GetComponent<CapsuleCollider>();

        rb.freezeRotation=true;


    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }
    void FixedUpdate()
    {
        CheckGrounded();
        CheckWater();
        UpdateVisuals();

        if (isInWater)
        {
            ApplyBuoyancy();
            MoveWater();
        }
        else
        {
            MoveLand();
        }
    }
    void CheckGrounded()
    {
        Vector3 origin=new Vector3(t.position.x,capsule.bounds.min.y+0.1f,t.position.z);
        isGrounded=Physics.CheckSphere(origin,0.25f,groundLayer);
    
    }
    void CheckWater()
    {
        Vector3 origin=new Vector3(t.position.x,t.position.y+100f,t.position.z);

        RaycastHit hit;

        if (Physics.Raycast(origin,Vector3.down,out hit, 2000f,waterLayer))
        {
            float depth=hit.point.y-t.position.y;
            isInWater=depth>0f;
            submersionDepth=isInWater?depth:0f;
        }
        else
        {
            isInWater=false;
            submersionDepth=0f;
        
        }
        if (anim!=null)anim.SetBool("InWater",isInWater);
    }
    void ApplyBuoyancy()
    {
        float upwardForce=Mathf.Min(submersionDepth*buoyancyStrength,maxBuoyancyForce);
        Vector3 velocity=rb.linearVelocity;

        velocity.y+=upwardForce*Time.fixedDeltaTime;
        velocity.y-=velocity.y*waterDrag*Time.fixedDeltaTime;

        rb.linearVelocity=velocity;
    }
    void MoveWater()
    {
        float h=Input.GetAxisRaw("Horizontal");
        float v=Input.GetAxisRaw("Vertical");

        Vector3 moveDir=(t.forward *v+t.right*h).normalized;
        Vector3 velocity=rb.linearVelocity;

        velocity.x=moveDir.x*walkSpeed;
        velocity.z=moveDir.z*walkSpeed;
        if (v!=0)
        {
            velocity.y=moveDir.y*walkSpeed;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            velocity.y=jumpForce*0.5f;
        }
        rb.linearVelocity=velocity;
        if (anim!=null) anim.SetFloat("Speed",(h!=0||v!=0)?walkSpeed:0f);
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
        if (anim!=null) anim.SetFloat("Speed",(h!=0||v!=0)?currentSpeed:0f);
    }
    void Jump()
    {
        Vector3 velocity=rb.linearVelocity;
        velocity.y=jumpForce;
        rb.linearVelocity=velocity;
    }
    void UpdateVisuals()
    {
        if(underwaterPostProcessing !=null)
        {
            float targetWeight=isInWater?1f:0f;
            underwaterPostProcessing.weight=Mathf.Lerp(underwaterPostProcessing.weight,targetWeight,Time.fixedDeltaTime*transitionSpeed);
        }
    }
}

