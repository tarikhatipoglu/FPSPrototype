using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pickup_items : MonoBehaviour
{
    public string itemName;
    public TextMeshProUGUI itemNameTxt;
    public bool boolClick;

    public bool InventoryBool;
    public GameObject currentObject = null;
    public InventorySystem _inventory;

    public void Awake()
    {
        
    }
    void Start()
    {
        itemNameTxt.enabled = false;
    }

    
    void Update()
    {
        if(Input.GetKey(KeyCode.F) && boolClick && InventoryBool)
        {
            _inventory.AddItem(currentObject);
            itemNameTxt.enabled = false;
            gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider itemEnter)
    {
        if(itemEnter.gameObject.tag == "Player")
        {
            currentObject = this.gameObject;
            itemNameTxt.enabled = true;
            itemNameTxt.text = "Press F to pick up " + itemName.ToString();
            boolClick = true;
        }
    }
    public void OnTriggerExit(Collider itemExit)
    {
        if(itemExit.gameObject.tag == "Player")
        {
            currentObject = null;
            itemNameTxt.enabled = false;
            itemNameTxt.text = "";
            boolClick = false;
        }
    }
}
