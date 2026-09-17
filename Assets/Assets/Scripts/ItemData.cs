using UnityEngine;

[CreateAssetMenu(fileName="New Item", menuName="Inventory/Item")]

public class ItemData:ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject model3D;
    public bool isStackable;
    public int maxStack=64;
    
}
