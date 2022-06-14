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

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 24; i++)
        {
            GameObject newSlot = Instantiate(slotExample, Vector3.zero, default, slotHolder.transform);
            newSlot.name = i.ToString();
            inventroySlot newInventroySlot = new inventroySlot(i, 0, newSlot);
            inventroySlots.Add(i, newInventroySlot);
            newSlot.SetActive(true);
            // newInventroySlot.slotButton.onClick.AddListener(() =>
            // {
            //     if (mouseDown == true)
            //     {
            //         inventroySlotSwaping = newInventroySlot.id;
            //         Transform from = newInventroySlot.slotUiElement.gameObject.transform.Find("SwapFrom");
            //         from.gameObject.SetActive(true);
            //     }
            // });
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
            if (value.gameObject.transform.IsChildOf(slotHolder.transform) && value.gameObject.name != "Text" && value.gameObject.name != "SwapFrom" && value.gameObject.name != "SwapTo")
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


    void SwapSlotItem(inventroySlot from, inventroySlot to) {
        InventoryItem itemFrom = from.item;
        int amountFrom = from.amountHolding;
        
        InventoryItem itemTo = to.item;
        int amountTo = to.amountHolding;

        to.item = itemFrom;
        to.amountHolding = amountFrom;
        
        from.item = itemTo;
        from.amountHolding = amountTo;

        print(from.item);
        print(to.item);
        

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

        if (Input.GetMouseButtonDown(0))
        {
            bool overSlot;
            int slotId;
            mouseDown = true;

            IsPointerOverInventroySlot(out overSlot, out slotId);
            if (overSlot)
            {
                inventroySlotSwaping = inventroySlots[slotId];
                GameObject from = inventroySlotSwaping.slotUiElement.transform.Find("SwapFrom").gameObject;
                from.SetActive(true);
            }
        }


        else if (Input.GetMouseButtonUp(0))
        {

            if (inventroySlotSwaping != null && slotHoveringOver != null)
            {
                if (inventroySlotSwaping != slotHoveringOver)
                {
                    //print(" From " + inventroySlotSwaping.id + " to " + slotHoveringOver.id);
                    SwapSlotItem(inventroySlotSwaping, slotHoveringOver);
                }

                GameObject from = inventroySlotSwaping.slotUiElement.transform.Find("SwapFrom").gameObject;
                from.SetActive(false);
                inventroySlotSwaping = null;
                GameObject to = slotHoveringOver.slotUiElement.transform.Find("SwapTo").gameObject;
                to.SetActive(false);
                slotHoveringOver = null;
            }
        }


    }
}


public class inventroySlot
{
    public int id;
    public int amountHolding = 0;
    InventoryItem _item;
    public InventoryItem item
    {
        get { return _item; }
        set
        {
            TextMeshProUGUI text = this.slotUiElement.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            if (text)
            {
                Debug.Log(value);
                if (value != null){
                text.text = value.name;
                } else {
                    text.text = "button";
                }
            }
            _item = value;
        }
    }

    public GameObject slotUiElement;
    public Button slotButton;
    public bool hasItem = false;

    public inventroySlot(int id, int amountHolding, GameObject slotUiElement)
    {
        this.id = id;
        this.amountHolding = amountHolding;
        this.slotUiElement = slotUiElement;
        this.slotButton = slotUiElement.GetComponent<Button>();
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
