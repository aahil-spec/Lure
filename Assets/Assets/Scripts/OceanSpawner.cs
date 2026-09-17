using UnityEngine;

public class OceanSpawner:MonoBehaviour
{
    public GameObject fishPrefab;
    public int numberOfFish=100;
    public Vector3 spawnArea=new Vector3(200f,30f,200f);

    void Start()
    {
        for (int i=0;i<numberOfFish;i++)
        {
            Vector3 randomPosition =new Vector3(
                Random.Range(-spawnArea.x/2,spawnArea.x/2),
                Random.Range(-spawnArea.y/2,spawnArea.y/2),
                Random.Range(-spawnArea.z/2,spawnArea.z/2)
            )+transform.position;

            Instantiate(fishPrefab,randomPosition,Quaternion.identity,transform);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color=new Color(0,1,0,0.3f);
        Gizmos.DrawCube(transform.position,spawnArea);
    }
    
}
