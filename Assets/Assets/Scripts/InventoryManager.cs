using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI[] hotbarCounts;

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
                if(hotbarCounts.Length>i&& hotbarCounts[i]!=null)
                {
                    hotbarCounts[i].text=slots[i].amount>1?slots[i].amount.ToString():"";
                }
            }
            else
            {
                hotbarIcons[i].sprite=null;
                hotbarIcons[i].color= new Color(1,1,1,0);
                if(hotbarCounts.Length>i&& hotbarCounts[i]!=null)
                {
                    hotbarCounts[i].text="";
                }
            }
        }
    }
    public bool HasItem(ItemData itemToCheck,int amountRequired)
    {
        int totalAmount=0;
        foreach(InventorySlot slot in slots)
        {
            if (slot.item==itemToCheck)
            {
                totalAmount+=slot.amount;

            }
        }
        return totalAmount>=amountRequired;
    }
    public void RemoveItem(ItemData itemToRemove,int amountToRemove)
    {
        int amountLeftToRemove=amountToRemove;
        for (int i=slots.Count-1; i>=0;i--)
        {
            if (slots[i].item==itemToRemove)
            {
                if (slots[i].amount>amountLeftToRemove)
                {
                    slots[i].amount-=amountLeftToRemove;
                    UpdateUI();
                    return;
                }
                else
                {
                    amountLeftToRemove-=slots[i].amount;
                    slots.RemoveAt(i);
                }
            }
        }
        UpdateUI();
    }
    
}
