using UnityEngine;
using UnityEngine.UI;

public class HotbarSelector:MonoBehaviour
{
    [Header("UI References")]
    public Transform hotbarPanel;

    private int selectedSlot=0;

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
    }
    void SelectSlot(int index)
    {
        selectedSlot=index;
        for (int i=0; i < hotbarPanel.childCount; i++)
        {
            Image slotImage = hotbarPanel.GetChild(i).GetComponent<Image>();
            if (slotImage!=null)
            {
                if (i==selectedSlot)
                    slotImage.color=Color.white;
                else
                    slotImage.color=new Color(0.5f,0.5f,0.5f,0.5f);

            }
        }
    }
    
}
