using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    public Transform playerBody;
    public Transform cameraPivot;

    [Header("Settings")]
    public float mouseSensitivity = 2f;
    public Vector3 thirdPersonOffset = new Vector3(0.6f, 0f, -3.5f);

    private float xRotation = 0f;
    private bool isFirstPerson = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1)) Cursor.lockState=CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState=CursorLockMode.None;

        if (Cursor.lockState !=CursorLockMode.Locked)return;

        float mouseX=Input.GetAxis("Mouse X")*mouseSensitivity;
        float mouseY=Input.GetAxis("Mouse Y")*mouseSensitivity;

        playerBody.Rotate(Vector3.up*mouseX);

        xRotation-=mouseY;
        xRotation=Mathf.Clamp(xRotation,-80f,80f);
        cameraPivot.localRotation=Quaternion.Euler(xRotation,0f,0f);

        if (Input.GetKeyDown(KeyCode.V)) isFirstPerson=!isFirstPerson;

        if (isFirstPerson)
        {
            transform.localPosition=Vector3.zero;
        }
        else
        {
            transform.localPosition=thirdPersonOffset;
        }
        transform.localRotation=Quaternion.identity;
    }
}