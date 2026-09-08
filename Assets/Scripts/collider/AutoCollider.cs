using UnityEngine;

public class AutoCollider:MonoBehaviour
{
    [ContextMenu("Generate City Colliders")]
    void GenerateColliders()
    {
        MeshFilter[]meshFilters=GetComponentsInChildren<MeshFilter>();
        foreach (var mf in meshFilters)
        {
            if (mf.GetComponent<MeshCollider>()==null)
            {
                MeshCollider mc =mf.gameObject.AddComponent<MeshCollider>();
                mc.convex=false;
            }
        }
        Debug.Log("Colliders generated for " + meshFilters.Length + " objects!");
    }
    
}
