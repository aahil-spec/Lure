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

    [Header("Camera Toggle")]
    public Transform cameraTransform;
    public Vector3 firstPersonPos=new Vector3(0f,0.7f,0.3f);
    public Vector3 thirdPersonPos=new Vector3(0f,1.5f,-4f);
    public bool isFirstPerson=false;
    public float camTransitionSpeed=8f;

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
    public Color underwaterColor=new Color(0f,0.4f,0.6f,1f);
    public float surfaceFogDensity=0.015f;
    public float deepFogDensity=0.15f;
    public float depthForMaxFog=20f;
    public Volume underwaterPostProcessing;
    public float transitionSpeed=5f;
    bool defaultFog;
    Color defaultFogColor;
    float defaultFogDensity;
    bool wasInWater;
    Camera mainCam;
    float defaultFarClipPlane;


    void Start()
    {
        t=this.transform;
        rb=GetComponent<Rigidbody>();
        capsule=GetComponent<CapsuleCollider>();

        rb.freezeRotation=true;

        Cursor.lockState=CursorLockMode.Locked;
        defaultFog=RenderSettings.fog;
        defaultFogColor=RenderSettings.fogColor;
        defaultFogDensity=RenderSettings.fogDensity;
        RenderSettings.fogMode=FogMode.ExponentialSquared;
        mainCam=Camera.main;
        if (mainCam!=null) defaultFarClipPlane=mainCam.farClipPlane;

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
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson=!isFirstPerson;
        }
        if (cameraTransform!=null)
        {
            Vector3 targetPos=isFirstPerson?firstPersonPos:thirdPersonPos;
            cameraTransform.localPosition=Vector3.Lerp(cameraTransform.localPosition,targetPos,Time.deltaTime*camTransitionSpeed);
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

    void LookAround()
    {

        rotationX+=Input.GetAxis("Mouse X")*sensitivity;
        rotationY+=Input.GetAxis("Mouse Y")*sensitivity;

        t.localRotation=Quaternion.Euler(-rotationY,rotationX,0);

    }
    void CheckGrounded()
    {
        Vector3 origin=new Vector3(t.position.x,capsule.bounds.min.y+0.1f,t.position.z);
        isGrounded=Physics.Raycast(origin,Vector3.down,groundCheckDistance+0.1f,groundLayer);
    
    }
    void CheckWater()
    {
        Vector3 origin=new Vector3(t.position.x,t.position.y+100f,t.position.z);

        RaycastHit hit;

        if (Physics.Raycast(origin,Vector3.down,out hit, 200f,waterLayer))
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
    void UpdateVisuals()
    {
        if (isInWater&&!wasInWater)
        {
            RenderSettings.fog=true;
            RenderSettings.fogColor=underwaterColor;
            if (mainCam!=null) mainCam.farClipPlane=40f;
            wasInWater=true;
        }
        else if (!isInWater&& wasInWater)
        {
            RenderSettings.fog=defaultFog;
            RenderSettings.fogColor=defaultFogColor;
            RenderSettings.fogDensity=defaultFogDensity;
            if (mainCam!=null) mainCam.farClipPlane=defaultFarClipPlane;
            wasInWater=false;
        }
        if (isInWater)
        {
            float depthPercentage=Mathf.Clamp01(submersionDepth/depthForMaxFog);
            RenderSettings.fogDensity=Mathf.Lerp(surfaceFogDensity,deepFogDensity,depthPercentage);

        }
        if (underwaterPostProcessing!=null)
        {
            float targetWeight=isInWater?1f:0f;
            underwaterPostProcessing.weight=Mathf.Lerp(underwaterPostProcessing.weight,targetWeight,Time.fixedDeltaTime*transitionSpeed);

        }
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
}
