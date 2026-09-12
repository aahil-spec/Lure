using UnityEngine;

public class AutoCollider:MonoBehaviour
{
    void Awake()
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
    }
    
}
