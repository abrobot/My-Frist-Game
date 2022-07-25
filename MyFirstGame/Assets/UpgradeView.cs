using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class UpgradeView : ViewWithMouse
{
    public UpgradeHandler upgradeHandler;

    public GameObject upgradeCardsHolder;
    public GameObject exampleUpgradeCard;
    public statImage[] statImages;


    public TextMeshProUGUI InfoCardNameText;
    public TextMeshProUGUI InfoCardTypeText;
    public TextMeshProUGUI InfoCardIncreaseByText;
    public TextMeshProUGUI InfoCardDevelepTimeText;
    public GameObject InfoCardCostContentObject;
    public GameObject InfoCardCostCardExample;




    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();
        if (this.isOpen)
        {
            bool overUpgradeCard;
            UpgradeCard upgradeCard;
            IsPointerOverInventroySlot(out overUpgradeCard, out upgradeCard);
            if (overUpgradeCard)
            {
                UpdateInfoCard(upgradeCard);
            }
        }
    }

    public UpgradeCard CreateNewUpgradeCard(UpgradeStack upgradeStack)
    {
        GameObject newUiCard = GameObject.Instantiate(exampleUpgradeCard, upgradeCardsHolder.transform);
        newUiCard.name = Random.Range(1, 999999999).ToString();
        UpgradeCard newCard = new UpgradeCard(newUiCard, upgradeStack, statImages);
        return newCard;
    }

    void UpdateInfoCard(UpgradeCard upgradeCard)
    {
        Upgrade upgrade = upgradeCard.currentUpgrade;
        InfoCardNameText.text = upgrade.name;
        InfoCardIncreaseByText.text = "Increase: " + upgrade.amount;
        InfoCardTypeText.text = "Type: " + upgradeCard.upgradeStack.stat.ToString();
        InfoCardDevelepTimeText.text = "Develep Time: NA";

        foreach (Transform child in InfoCardCostContentObject.transform)
        {
            if (child != InfoCardCostCardExample.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        foreach (UpgradeCost cost in upgrade.upgradeCosts)
        {
            GameObject newCostCard = GameObject.Instantiate(InfoCardCostCardExample);
            newCostCard.transform.SetParent(InfoCardCostContentObject.transform, false);
            newCostCard.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = cost.item.name;
            Image image = newCostCard.transform.Find("Image").GetComponent<Image>();
            image.sprite = cost.item.sprite;
            newCostCard.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = cost.amount.ToString();
            newCostCard.SetActive(true);
        }
    }

    public void IsPointerOverInventroySlot(out bool overUpgradeCard, out UpgradeCard upgradeCard)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult value in results)
        {
            if (value.gameObject.transform.parent == upgradeCardsHolder.transform && value.gameObject != exampleUpgradeCard)
            {
                overUpgradeCard = true;
                upgradeCard = upgradeHandler.allUpgradeCards[value.gameObject.name];
                return;
            }
        }
        overUpgradeCard = false;
        upgradeCard = null;
    }

}



public enum stat { Damage, Health, Resistance };

public class UpgradeCard
{
    public UpgradeStack upgradeStack;
    public Upgrade currentUpgrade;
    public int currentStackIndex;
    public GameObject cardUiElement;

    public Button button;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI statText;
    public TextMeshProUGUI maxUpgradeText;
    public Image statSprite;

    public bool clickDebounce = false;


    public UpgradeCard(GameObject cardUiElement, UpgradeStack upgradeStack, statImage[] statImages)
    {
        this.upgradeStack = upgradeStack;

        this.button = cardUiElement.GetComponent<Button>();
        this.nameText = cardUiElement.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        this.statText = cardUiElement.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
        this.maxUpgradeText = cardUiElement.transform.Find("Max").GetComponent<TextMeshProUGUI>();
        this.statSprite = cardUiElement.transform.Find("StatSprite").GetComponent<Image>();

        foreach (statImage statImage in statImages)
        {
            if (statImage.stat == upgradeStack.stat)
            {
                this.statSprite.sprite = statImage.image;
            }
        }

        this.cardUiElement = cardUiElement;
        cardUiElement.SetActive(true);

        this.currentStackIndex = -1;

        NextUpgrade();
    }


    public void NextUpgrade()
    {
        if (currentUpgrade.Equals(upgradeStack.stack[upgradeStack.stack.Length - 1]))
        {
            this.button.onClick.RemoveAllListeners();
            maxUpgradeText.gameObject.SetActive(true);
            statText.gameObject.SetActive(false);
            nameText.text = upgradeStack.name;
            return;
        }


        currentStackIndex += 1;
        currentUpgrade = upgradeStack.stack[currentStackIndex];


        nameText.text = currentUpgrade.name;
        statText.text = upgradeStack.stat.ToString() + " + " + currentUpgrade.amount;

        //cardUiElement.name = Random.Range(1, 999999999).ToString();
    }
}