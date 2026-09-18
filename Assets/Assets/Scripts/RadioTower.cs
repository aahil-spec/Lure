using UnityEngine;

public class RadioTower:MonoBehaviour
{
    [Header("Repair Requirements")]
    public ItemData scrapMetalItem;
    public int requiredScrap=5;
    private bool isFixed=false;

    public void Interact(InventoryManager playerInventory)
    {
        if(isFixed) return;
        if (playerInventory.HasItem(scrapMetalItem,requiredScrap))
        {
            playerInventory.RemoveItem(scrapMetalItem,requiredScrap);
            isFixed=true;
            GameUI.Instance.HidePrompt();
            GameUI.Instance.ShowNotification("RADIO TOWER FIXED! YOU WIN!");
        }
        else
        {
            GameUI.Instance.ShowNotification($"Not enough parts! You need {requiredScrap} Scrap Metal.");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&& !isFixed)
        {
            GameUI.Instance.ShowPrompt($"Press E to repair Radio Tower");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameUI.Instance.HidePrompt();
        }
    }
    
}
