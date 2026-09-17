using UnityEngine;

public class ObjectiveMachine:MonoBehaviour
{
    [Header("Repair Requirements")]
    public ItemData scrapMetalItem;
    public int requiredScrap=5;

    public void Interact(InventoryManager playerInventory)
    {
        if(playerInventory.HasItem(scrapMetalItem,requiredScrap))
        {
            playerInventory.RemoveItem(scrapMetalItem,requiredScrap);
            Debug.Log("<color=green>MACHINE REPAIRED! YOU WIN!</color>");
        }
        else
        {
            Debug.Log($"<color=red>Not enough parts! You need {requiredScrap} Scrap Metal.</color>");
        }
    }
    
}
