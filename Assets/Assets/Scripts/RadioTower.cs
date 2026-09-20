using UnityEngine;

public class RadioTower:MonoBehaviour
{
    [Header("Repair Requirements")]
    public InventoryManager inventory;
    public ItemData scrapMetalItem;
    public int requiredScrap=5;
    private bool playerInRange=false;
    private bool isFixed=false;

    void Update()

    {
        if (playerInRange && !isFixed && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory !=null && inventory.HasItem(scrapMetalItem,requiredScrap))
            {
                inventory.RemoveItem(scrapMetalItem,requiredScrap);
                isFixed=true;
                GameUI.Instance.HidePrompt();
                GameUI.Instance.ShowNotification("RADIO TOWER FIXED! YOU WIN!");
            }
            else
            {
                GameUI.Instance.ShowNotification($"Not enough parts! You need{requiredScrap} Scrap Metal.");

            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&& !isFixed)
        {
            playerInRange=true;
            GameUI.Instance.ShowPrompt($"Press E to repair Radio Tower");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange=false;
            GameUI.Instance.HidePrompt();
        }
    }
    
}
