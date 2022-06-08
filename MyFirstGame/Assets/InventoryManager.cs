using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventroyUIelement;
    public GameObject slotHolder;
    public GameObject slotExample;

    public Dictionary<int, inventroySlot> inventroySlots = new Dictionary<int, inventroySlot>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 24; i++)
        {
            GameObject newSlot = Instantiate(slotExample, Vector3.zero, default, slotHolder.transform);
            print(newSlot);
            newSlot.name = i.ToString();
            inventroySlots.Add(i, new inventroySlot(i, 0, newSlot));
            newSlot.SetActive(true);
        }
    }

    void Add(InventoryItem item, int amount)
    {
        for (int i = 0; i < inventroySlots.Count; i++)
        {
            inventroySlot currentSlot = inventroySlots[i];
            if (!currentSlot.hasItem)
            {
                currentSlot.item = item;
                currentSlot.amountHolding = amount;
                return;
            }
        }
    }

    bool hasdone = false;
    // Update is called once per frame
    void Update()
    {
        if (Mathf.RoundToInt(Time.time) == 5 && hasdone == false) {
            Add(new InventoryItem(new GameObject("RedSlime"), true, "RedSlime"), 10);
            hasdone = true;
        }
    }
}



public class inventroySlot
{
    public int id;
    public int amountHolding = 0;
    public InventoryItem _item;
    public InventoryItem item
    {
        get {return _item;}
        set {
             TextMeshProUGUI text = this.slotUiElement.transform.Find("Text").GetComponent<TextMeshProUGUI>();
             if (text) {
                 text.text = value.name;
             }
            _item = value;
        }
    }

    public GameObject slotUiElement;
    public bool hasItem = false;

    public inventroySlot(int id, int amountHolding, GameObject slotUiElement)
    {
        this.id = id;
        this.amountHolding = amountHolding;
        this.slotUiElement = slotUiElement;
    }
}


public class InventoryItem
{
    public GameObject model;
    public bool stackable;
    public string name;

    public InventoryItem(GameObject model, bool stackable, string name)
    {
        this.model = model;
        this.stackable = stackable;
        this.name = name;
    }
}
