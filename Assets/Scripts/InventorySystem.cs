using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public GameObject[] inventorySlots = new GameObject[10];

    public void Start()
    {
        
    }
    public void AddItem(GameObject item)
    {
        bool itemAdded = false;
        for(int a=0; a < inventorySlots.Length; a++)
        {
            if(inventorySlots[a] == null)
            {
                inventorySlots[a] = item;
                Debug.Log(item.name + "is added");
                itemAdded = true;
                break;
            }
        }

        if (!itemAdded)
        {
            Debug.Log("Inventory is full - item not added");
        }
    }
}
