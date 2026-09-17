using UnityEngine;

public class FloatingItem:MonoBehaviour
{
    [Header("Settings")]
    public float spinSpeed=100f;
    public float bobSpeed=2f;
    public float bobHeight=0.15f;

    private Vector3 startPos;

    void Start()
    {
        startPos=transform.localPosition;
    }
    void Update()
    {
        transform.Rotate(Vector3.up,spinSpeed*Time.deltaTime,Space.World);

        float newY=startPos.y+Mathf.Sin(Time.time*bobSpeed)*bobHeight;
        transform.localPosition=new Vector3(startPos.x,newY,startPos.z);
    }
    
}
