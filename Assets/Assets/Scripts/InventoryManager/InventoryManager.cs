using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class InventorySlot
{
    public ItemData item;
    public int amount;

    public InventorySlot(ItemData newItem,int newAmount)
    {
        item = newItem;
        amount=newAmount;
    }
    
}
public class InventoryManager:MonoBehaviour
{
    [Header("Inventory Settings")]
    public int maxSlots=20;
    public int hotbarSlots=5;

    [Header("Current Inventory")]
    public List<InventorySlot> slots=new List<InventorySlot>();
    public Image[] hotbarIcons;

    public bool AddItem(ItemData itemToAdd,int amountToAdd)
    {
        if (itemToAdd.isStackable)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.item==itemToAdd)
                {
                    int spaceLeft=itemToAdd.maxStack-slot.amount;
                    if (amountToAdd<=spaceLeft)
                    {
                        slot.amount+=amountToAdd;
                        Debug.Log($"Stacked{amountToAdd}{itemToAdd.itemName}s. Total:{slot.amount}");
                        UpdateUI();
                        return true;

                    }

                }
  
            }
        }
        if (slots.Count<maxSlots)
        {
            slots.Add(new InventorySlot(itemToAdd,amountToAdd));
            Debug.Log($"Added new slot with{amountToAdd}{itemToAdd.itemName}(s)");
            UpdateUI();
            return true;
        }
        Debug.Log("Inventory is full!");
        return false;

    }
    public void UpdateUI()
    {
        for (int i=0;i<hotbarIcons.Length;i++)
        {
            if (i<slots.Count)
            {
                hotbarIcons[i].sprite=slots[i].item.icon;
                hotbarIcons[i].color=new Color(1,1,1,1);
            }
            else
            {
                hotbarIcons[i].sprite=null;
                hotbarIcons[i].color= new Color(1,1,1,0);
            }
        }
    }
    
}
