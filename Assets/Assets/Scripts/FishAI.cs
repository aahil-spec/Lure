using UnityEngine;

public class FishAI:MonoBehaviour
{
    public float swimSpeed=2f;
    public float turnSpeed=1.5f;
    public float changeTargetDistance=2f;
    public float roamRadius=15f;
    public float obsracleCheckDistance=3f;
    public LayerMask obstacleLayer;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition=transform.position;
        GetNewTarget();
    }
    void Update()
    {
        transform.position+=transform.forward*swimSpeed *Time.deltaTime;
        RaycastHit hit;
        bool hitForward =Physics.SphereCast(transform.position,0.5f,transform.forward,out hit,obsracleCheckDistance,obstacleLayer);
        bool hitFloor=Physics.Raycast(transform.position,Vector3.down,out hit,obsracleCheckDistance,obstacleLayer);

        if (hitForward||hitFloor)
        {
            if (hitFloor)targetPosition=transform.position+new Vector3(Random.Range(-5f,5f),5f,Random.Range(-5f,5f));
            else GetNewTarget();
        }
        
        Vector3 direction=targetPosition-transform.position;
        if(direction!=Vector3.zero)
        {
            Quaternion targetRotation=Quaternion.LookRotation(direction);
            transform.rotation=Quaternion.Slerp(transform.rotation,targetRotation,Time.deltaTime*turnSpeed);
        }
        if (Vector3.Distance(transform.position,targetPosition)<changeTargetDistance)
        {
            GetNewTarget();
        }
    }
    void GetNewTarget()
    {
        targetPosition=startPosition+new Vector3(
            Random.Range(-roamRadius,roamRadius),
            Random.Range(-roamRadius/2f,roamRadius/2f),
            Random.Range(-roamRadius,roamRadius)
        );
    }
}