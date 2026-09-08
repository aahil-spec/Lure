using UnityEngine;

public class FloorSpawner:MonoBehaviour
{
    public GameObject plantPrefab;
    public int numberOfPlants=200;
    public Vector3 spawnArea=new Vector3(200f,10f,200f);
    public LayerMask groundLayer;

    void Start()
    {
        for (int i=0; i<numberOfPlants; i++)
        {
            Vector3 randomPos=transform.position+new Vector3(
                Random.Range(-spawnArea.x/2,spawnArea.z/2),
                0,
                Random.Range(-spawnArea.z/2,spawnArea.z/2)

            );
            RaycastHit hit;
            if (Physics.Raycast(randomPos,Vector3.down,out hit,Mathf.Infinity,groundLayer))
            {
                Instantiate(plantPrefab,hit.point,Quaternion.Euler(0,Random.Range(0,360),0),transform);

            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color=new Color(0,1,0,0.3f);
        Gizmos.DrawCube(transform.position,spawnArea);
    }
    
}
