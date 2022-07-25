using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public ViewWithMouse inventoryView;
    public GameObject inventroyUIelement;
    public GameObject slotHolder;
    public GameObject slotExample;

    public bool mouseDown = false;
    public inventroySlot inventroySlotSwaping;
    public inventroySlot slotHoveringOver;

    public InventoryItemSO RedBlobDropSO;

    public Dictionary<int, inventroySlot> inventroySlots = new Dictionary<int, inventroySlot>();

    Dictionary<string, InventoryItemSO> gameitems;

    void Start()
    {
        gameitems = new Dictionary<string, InventoryItemSO>();
        foreach (InventoryItemSO item in GetPrefabsFromFolder.GetInventroySO("InventoryItemSOs"))
        {
            gameitems.Add(item.name, item);
        }
        CreateInventorySlots(24);
    }


    void CreateInventorySlots(int numberOfSlots)
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject newSlot = Instantiate(slotExample, Vector3.zero, default, slotHolder.transform);
            newSlot.name = i.ToString();
            inventroySlot newInventroySlot = new inventroySlot(i, 0, newSlot);
            inventroySlots.Add(i, newInventroySlot);
            newSlot.SetActive(true);
        }
    }


    public void IsPointerOverInventroySlot(out bool overSlot, out int slotId)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult value in results)
        {
            if (value.gameObject.transform.IsChildOf(slotHolder.transform) && value.gameObject.name != "Text" && value.gameObject.name != "SwapFrom" && value.gameObject.name != "SwapTo" && value.gameObject.name != "Count" && value.gameObject.name != "ItemSprite")
            {
                overSlot = true;
                slotId = int.Parse(value.gameObject.name);
                return;
            }
        }
        overSlot = false;
        slotId = 0;
    }

    public void Add(InventoryItemSO item, int amount)
    {
        inventroySlot firstAvailableSlot = null;
        for (int i = 0; i < inventroySlots.Count; i++)
        {
            inventroySlot currentSlot = inventroySlots[i];
            if (currentSlot.hasItem)
            {
                if (currentSlot.item == item)
                {
                    currentSlot.amountHolding += amount;
                    return;
                }
            }
            else
            {
                if (firstAvailableSlot == null)
                {
                    firstAvailableSlot = currentSlot;
                }
            }
        }
        firstAvailableSlot.item = item;
        firstAvailableSlot.amountHolding = amount;
    }

    public bool Remove(InventoryItemSO item, int amount)
    {
        for (int i = 0; i < inventroySlots.Count; i++)
        {

            inventroySlot currentSlot = inventroySlots[i];

            if (currentSlot.item == item)
            {
                if (currentSlot.amountHolding >= amount)
                {
                    currentSlot.amountHolding -= amount;
                    if (currentSlot.amountHolding <= 0)
                    {
                        currentSlot.Reset();
                    }
                    return true;
                }

            }
        }
        return false;
    }

    public bool removeFullListWithCheck(UpgradeCost[] values)
    {
        foreach (UpgradeCost cost in values)
        {
            bool foundItem = false;
            foreach (inventroySlot currentSlot in inventroySlots.Values)
            {
                foundItem = false;
                if (currentSlot.item == cost.item)
                {
                    if (currentSlot.amountHolding >= cost.amount)
                    {
                        foundItem = true;
                        break;
                    }
                }
            }
            if (!foundItem)
            {
                return false;
            }
        }

        foreach (UpgradeCost cost in values)
        {
            Remove(cost.item, cost.amount);
        }
        return true;
    }
    void SwapSlotItem(inventroySlot from, inventroySlot to)
    {
        InventoryItemSO tempItem = from.item;
        int tempAmount = from.amountHolding;

        from.item = to.item;
        from.amountHolding = to.amountHolding;

        to.item = tempItem;
        to.amountHolding = tempAmount;
    }

    void CheckMouseStatus()
    {

        // Check if mouse is over slot
        if (Input.GetMouseButton(0))
        {
            bool overSlot;
            int slotId;
            mouseDown = true;

            IsPointerOverInventroySlot(out overSlot, out slotId);
            if (overSlot)
            {
                if (slotHoveringOver != null && slotHoveringOver != inventroySlots[slotId])
                {
                    slotHoveringOver.slotUiElement.transform.Find("SwapTo").gameObject.SetActive(false);
                }

                slotHoveringOver = inventroySlots[slotId];
                GameObject to = slotHoveringOver.slotUiElement.transform.Find("SwapTo").gameObject;
                to.SetActive(true);


            }
        }

        // check if mouse click on slot
        if (Input.GetMouseButtonDown(0))
        {

            if (slotHoveringOver != null && inventroySlotSwaping == null)
            {
                inventroySlotSwaping = slotHoveringOver;
                GameObject from = inventroySlotSwaping.slotUiElement.transform.Find("SwapFrom").gameObject;
                from.SetActive(true);
            }
        }

        //check if mouse click end
        if (Input.GetMouseButtonUp(0))
        {

            if (inventroySlotSwaping != null && slotHoveringOver != null)
            {
                if (inventroySlotSwaping != slotHoveringOver)
                {
                    SwapSlotItem(inventroySlotSwaping, slotHoveringOver);
                }
            }

            if (inventroySlotSwaping != null)
            {
                GameObject from = inventroySlotSwaping.slotUiElement.transform.Find("SwapFrom").gameObject;
                from.SetActive(false);
                inventroySlotSwaping = null;
            }

            if (slotHoveringOver != null)
            {
                GameObject to = slotHoveringOver.slotUiElement.transform.Find("SwapTo").gameObject;
                to.SetActive(false);
                slotHoveringOver = null;
            }
        }
    }


    // bool hasdone = false;
    // Update is called once per frame
    void Update()
    {
        // if (Mathf.RoundToInt(Time.time) == 5 && hasdone == false)
        // {
        //     Add(RedBlobDropSO, 1000);
        //     hasdone = true;
        // }
        if (inventoryView.isOpen)
        {
            CheckMouseStatus();
        }
    }


    public InventoryItemSO translateNameToInventoryItem(string name)
    {
        return gameitems[name];
    }
}


public class inventroySlot
{
    public int id;
    int _amountHolding = 0;
    InventoryItemSO _item;

    public GameObject slotUiElement;
    public Button slotButton;
    public GameObject countElement;

    public bool hasItem = false;

    public void Reset()
    {
        item = null;
    }


    public inventroySlot(int id, int amountHolding, GameObject slotUiElement)
    {
        this.id = id;
        this.countElement = slotUiElement.transform.Find("Count").gameObject;
        this.amountHolding = amountHolding;
        this.slotUiElement = slotUiElement;
        this.slotButton = slotUiElement.GetComponent<Button>();
    }

    public InventoryItemSO item
    {
        get { return _item; }
        set
        {
            TextMeshProUGUI text = this.slotUiElement.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            Image itemSprite = this.slotUiElement.transform.Find("ItemSprite").GetComponent<Image>();

            if (text)
            {
                if (value != null)
                {
                    text.text = value.itemName;
                    itemSprite.sprite = value.sprite;
                    itemSprite.gameObject.SetActive(true);
                    hasItem = true;
                }
                else
                {
                    itemSprite.sprite = null;
                    itemSprite.gameObject.SetActive(false);
                    text.text = "Button";
                    amountHolding = 0;
                    hasItem = false;
                }
            }
            _item = value;
        }
    }

    public int amountHolding
    {
        get { return _amountHolding; }
        set
        {
            if (value != 0 && value > 1)
            {
                this.countElement.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = value.ToString();
                this.countElement.SetActive(true);
            }
            else
            {
                this.countElement.SetActive(false);
            }
            _amountHolding = value;
        }
    }
}

// public class InventoryItem
// {
//     public GameObject model;
//     public bool stackable;
//     public string name;

//     public InventoryItem(GameObject model, bool stackable, string name)
//     {
//         this.model = model;
//         this.stackable = stackable;
//         this.name = name;
//     }
// }
