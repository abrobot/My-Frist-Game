using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeHandler : MonoBehaviour
{
    public UpgradeStack[] upgradeStacks;
    public Dictionary<string, UpgradeCard> allUpgradeCards = new Dictionary<string, UpgradeCard>();


    public List<UpgradeCard> upgradeCards = new List<UpgradeCard>();
    public UpgradeView upgradeView;
    public InventoryManager inventoryManager;
    public PlayerStatus playerStatus;


    // void ReorderStacks()
    // {
    //     for (int i = 0; i < upgradeStacks.Length; i++) {
    //         UpgradeStack currentUpgradeStack = upgradeStacks[i];
    //         currentUpgradeStack.orderedStack = new List<Upgrade>();

    //         foreach (Upgrade upgrade in currentUpgradeStack.stack) {
    //             if (currentUpgradeStack.orderedStack.Count == 0) {
    //                 currentUpgradeStack.orderedStack.Add(upgrade);
    //                 continue;
    //             }
    //             for (int idex = 0; i < currentUpgradeStack.orderedStack.Count; i++) {
    //                 if (currentUpgradeStack.orderedStack[idex].positionInStack < upgrade.positionInStack) {

    //                     if (currentUpgradeStack.orderedStack[idex + 1].positionInStack > upgrade.positionInStack) {
    //                         currentUpgradeStack.orderedStack.Insert(idex, upgrade); 
    //                     }
    //                 }
    //             }
    //         }

    //     }
    // }


    void Start()
    {

        foreach (UpgradeStack upgradeStack in upgradeStacks)
        {
            UpgradeCard newUpgradeCard = upgradeView.CreateNewUpgradeCard(upgradeStack);

            newUpgradeCard.button.onClick.AddListener(() =>
            {
                if (newUpgradeCard.clickDebounce != true)
                {
                    newUpgradeCard.clickDebounce = true;
                    newUpgradeCard.button.interactable = false;
                    Game.instance.WaitFor(newUpgradeCard.currentUpgrade.name + " click Deboundce" + newUpgradeCard.currentUpgrade.name + newUpgradeCard.upgradeStack.name.ToString(), .3f, () => {
                        newUpgradeCard.clickDebounce = false;
                        newUpgradeCard.button.interactable = true;
                    });
                    PurchaseUpgrade(newUpgradeCard);
                }
            });

            upgradeCards.Add(newUpgradeCard);
            allUpgradeCards.Add(newUpgradeCard.cardUiElement.name, newUpgradeCard);
        }

    }


    bool PurchaseUpgrade(UpgradeCard upgradeCard)
    {

        bool success = inventoryManager.removeFullListWithCheck(upgradeCard.currentUpgrade.upgradeCosts);
        if (!success)
        {
            upgradeCard.button.image.color = new Color(.9f, .6f, .6f, 1);;
            Game.instance.WaitFor(upgradeCard.currentUpgrade.name + "Upgrade Butten Color Change red", .2f, () =>
            {
                upgradeCard.button.GetComponent<Image>().color = Color.white;
            });
            return false;
        }

        upgradeCard.button.image.color = new Color(.6f, .9f, .6f, 1);;
        Game.instance.WaitFor(upgradeCard.currentUpgrade.name + "Upgrade Butten Color Change green", .2f, () =>
        {
            upgradeCard.button.GetComponent<Image>().color = Color.white;
        });

        switch (upgradeCard.upgradeStack.stat)
        {
            case stat.Damage:
                playerStatus.damage += upgradeCard.currentUpgrade.amount;
                break;
            case stat.Health:
                playerStatus.AddMaxHealth(upgradeCard.currentUpgrade.amount);
                break;
            case stat.Resistance:
            playerStatus.resistance += upgradeCard.currentUpgrade.amount;
                break;
        }
        upgradeCard.NextUpgrade();
        return true;
    }


}


[System.Serializable]
public struct Upgrade
{
    public string name;
    public int amount;

    public UpgradeCost[] upgradeCosts;
}

[System.Serializable]
public struct UpgradeCost
{
    public InventoryItemSO item;
    public int amount;

    public UpgradeCost(InventoryItemSO item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}

[System.Serializable]
public struct UpgradeStack
{
    public string name;
    public stat stat;

    public Upgrade[] stack;
}


[System.Serializable]
public struct statImage{
    public stat stat;
    public Sprite image;

}