using UnityEngine;

public class ItemPickup:MonoBehaviour
{
    public ItemData item;
    public int amount=1;

    private bool playerInRange=false;
    private InventoryManager playerInventory;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            bool wasPickedUp=playerInventory.AddItem(item, amount);
            if (wasPickedUp)
            {
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange=true;
            playerInventory=other.GetComponent<InventoryManager>();

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange=false;
            playerInventory=null;
        }
    }
    
}
