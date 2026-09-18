using UnityEngine;

public class LootChest:MonoBehaviour
{
    public Animator chestAnimator;
    public GameObject[] hiddenItems;

    private bool playerInRange=false;
    private bool isOpened=false;

    public int scrapStored=0;

    void Update()
    {
        if (playerInRange && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            isOpened=true;
            chestAnimator.SetTrigger("Open");
            foreach (GameObject item in hiddenItems)
            {
                if (item!=null) item.SetActive(true);
            }
            GameUI.Instance.HidePrompt();
            GameUI.Instance.ShowNotification("Chest Opened!");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isOpened)
        {
            playerInRange=true;
            GameUI.Instance.ShowPrompt("Press E to open Chest");
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
    public void DepositScrap()
    {
        scrapStored++;
        GameUI.Instance.ShowNotification($"Scrap Deposited! ({scrapStored} total)");
    }
    
}
