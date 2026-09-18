using UnityEngine;
using UnityEngine.UI;

public class HotbarSelector:MonoBehaviour
{
    [Header("UI References")]
    public Transform hotbarPanel;
    private int selectedSlot=0;

    [Header("Goggles System")]
    public InventoryManager inventory;
    public ItemData scubaGogglesData;
    public GameObject gogglesVision;

    public GameObject wornGogglesModel;
    private bool isWearingGoggles=false;

    [Header("Water Settings")]
    public Transform mainCamera;
    public Transform waterObject;

    [Header("Scrap & Chest System")]
    public ItemData scrapMetalData;
    public float interactionRange=4f;

    void Start()
    {
        SelectSlot(selectedSlot);
    }
    void Update()
    {
        float scroll=Input.GetAxis("Mouse ScrollWheel");
        if (scroll>0f)
        {
            selectedSlot--;
            if (selectedSlot<0) selectedSlot=hotbarPanel.childCount-1;
            SelectSlot(selectedSlot);
        }
        else if (scroll<0f)
        {
            selectedSlot++;
            if (selectedSlot>=hotbarPanel.childCount) selectedSlot=0;
            SelectSlot(selectedSlot);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSlot(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) SelectSlot(6);

        if (Input.GetMouseButtonDown(0))
        {
            if (inventory!=null && selectedSlot<inventory.slots.Count)
            {
                ItemData selectedItem=inventory.slots[selectedSlot].item;
                if (selectedItem==scubaGogglesData)
                {
                    isWearingGoggles=true;
                    if(wornGogglesModel!=null) wornGogglesModel.SetActive(true);
                    inventory.RemoveItem(scubaGogglesData,1);
                }
                else if(selectedItem==scrapMetalData)
                {
                    Ray ray=new Ray(mainCamera.position,mainCamera.forward);
                    if (Physics.Raycast(ray,out RaycastHit hit,interactionRange))
                    {
                        LootChest chest=hit.collider.GetComponent<LootChest>();
                        if (chest!=null)
                        {
                            chest.DepositScrap();
                            inventory.RemoveItem(scrapMetalData,1);
                        }
                    }
                }
            }
        }
        if (mainCamera!=null && waterObject!=null && gogglesVision!=null)
        {
            bool isUnderwater=mainCamera.position.y<waterObject.position.y;
            bool hasGoggles=isWearingGoggles || inventory!=null &&selectedSlot<inventory.slots.Count && inventory.slots[selectedSlot].item==scubaGogglesData;
            gogglesVision.SetActive(isUnderwater && hasGoggles);
        }
    }
    void SelectSlot(int index)
    {
        selectedSlot=index;
        for (int i=0; i < hotbarPanel.childCount; i++)
        {
            Image slotImage = hotbarPanel.GetChild(i).GetComponent<Image>();
            if (slotImage!=null)
            {
                slotImage.color=new Color(1f,1f,1f,0.5f);
            }
            Outline slotOutline=hotbarPanel.GetChild(i).GetComponent<Outline>();
            if (slotOutline!=null)
            {
                slotOutline.enabled=(i==selectedSlot);

            }
        }
    }
}
