using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventroyUIelement;
    public GameObject slotHolder;
    public GameObject slotExample;

    public bool mouseDown = false;
    public inventroySlot inventroySlotSwaping;
    public inventroySlot slotHoveringOver;

    public Dictionary<int, inventroySlot> inventroySlots = new Dictionary<int, inventroySlot>();


    void Start()
    {
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
            if (value.gameObject.transform.IsChildOf(slotHolder.transform) && value.gameObject.name != "Text" && value.gameObject.name != "SwapFrom" && value.gameObject.name != "SwapTo" && value.gameObject.name != "Count")
            {
                overSlot = true;
                slotId = int.Parse(value.gameObject.name);
                return;
            }
        }
        overSlot = false;
        slotId = 0;
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


    void SwapSlotItem(inventroySlot from, inventroySlot to)
    {

        //print(from.amountHolding);  
        //print(to.amountHolding);  

        InventoryItem tempItem = from.item;
        int tempAmount = from.amountHolding;

        print(from.amountHolding);
        from.item = to.item;
        from.amountHolding = to.amountHolding;
        print(from.amountHolding);

        to.item = tempItem;
        to.amountHolding = tempAmount;


        //print(from.amountHolding);  
        //print(to.amountHolding);  

        // InventoryItem itemFrom = from.item;
        // int amountFrom = from.amountHolding;

        // InventoryItem itemTo = to.item;
        // int amountTo = to.amountHolding;

        // to.item = itemFrom;
        // to.amountHolding = amountFrom;

        // from.item = itemTo;
        // from.amountHolding = amountTo;
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


    bool hasdone = false;
    // Update is called once per frame
    void Update()
    {
        if (Mathf.RoundToInt(Time.time) == 5 && hasdone == false)
        {
            Add(new InventoryItem(new GameObject("RedSlime"), true, "RedSlime"), 10);
            hasdone = true;
        }

        CheckMouseStatus();
    }
}


public class inventroySlot
{
    public int id;
    int _amountHolding = 0;
    InventoryItem _item;

    public GameObject slotUiElement;
    public Button slotButton;
    public GameObject countElement;

    public bool hasItem = false;

    public inventroySlot(int id, int amountHolding, GameObject slotUiElement)
    {
        this.id = id;
        this.countElement = slotUiElement.transform.Find("Count").gameObject;
        this.amountHolding = amountHolding;
        this.slotUiElement = slotUiElement;
        this.slotButton = slotUiElement.GetComponent<Button>();
    }

    public InventoryItem item
    {
        get { return _item; }
        set
        {
            TextMeshProUGUI text = this.slotUiElement.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            if (text)
            {
                if (value != null)
                {
                    text.text = value.name;
                }
                else
                {
                    text.text = "button";
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
            //Debug.Log(value);
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
