using UnityEngine;

public class CameraManager:MonoBehaviour
{
    [Header("Target")]
    public Transform playerBody;

    [Header("Settings")]
    public float mouseSensitivity=2f;
    public KeyCode switchViewKey=KeyCode.V;

    [Header("Offsets")]
    public float pivotHeight=1.4f;
    public Vector3 firstPersonOffset=new Vector3(0f,0f,0.15f);
    public Vector3 thirdPersonOffset=new Vector3(0.5f,0f,-3f);

    private bool isFirstPerson=false;
    private float xRotation=0f;
    private float yRotation=0f;

    void Start()
    {
        LockMouse();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) LockMouse();
        if (Input.GetKeyDown(KeyCode.Escape)) UnlockMouse();

        if (Cursor.lockState==CursorLockMode.Locked)
        {
            if (Input.GetKeyDown(switchViewKey)) isFirstPerson=!isFirstPerson;
            float mouseX=Input.GetAxis("Mouse X")*mouseSensitivity;
            float mouseY=Input.GetAxis("Mouse Y")*mouseSensitivity;

            yRotation+=mouseX;
            xRotation-=mouseY;
            xRotation=Mathf.Clamp(xRotation,-80f,80f);
            transform.rotation=Quaternion.Euler(xRotation,yRotation,0);
            if (playerBody!=null)
            {
                playerBody.rotation=Quaternion.Euler(0,yRotation,0);

            }
            Vector3 currentOffset=isFirstPerson?firstPersonOffset:thirdPersonOffset;
            Vector3 pivotPoint=playerBody.position+Vector3.up*pivotHeight;
            transform.position=pivotPoint+transform.rotation*currentOffset;

        }
    }
    void LockMouse()
    {
        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible=false;
    }
    void UnlockMouse()
    {
        Cursor.lockState=CursorLockMode.None;
        Cursor.visible=true;
    }
    
}
