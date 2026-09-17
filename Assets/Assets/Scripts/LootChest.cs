using UnityEngine;

public class LootChest:MonoBehaviour
{
    public Animator chestAnimator;
    public GameObject playerGoggles;

    private bool playerInRange=false;
    private bool isOpened=false;

    void Update()
    {
        if (playerInRange && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            isOpened=true;
            chestAnimator.SetTrigger("Open");
            if (playerGoggles!=null)
            {
                playerGoggles.SetActive(true);
            }
            Debug.Log("Chest Opened! Goggles Equipped.");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))playerInRange=true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange=false;
    }
    
}
