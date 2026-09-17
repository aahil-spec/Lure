using UnityEngine;

public class ItemPickup:MonoBehaviour
{
    public ItemData item;
    public int amount=1;

    
    public void PickUp(InventoryManager playerInventory)
    {
        bool wasPickedUp=playerInventory.AddItem(item, amount);
        if(wasPickedUp)
        {
            Destroy(gameObject);
        }
    }  
}
