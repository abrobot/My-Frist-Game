using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionHandler : MonoBehaviour
{
    public PlayerStatus playerStatus;

    List<evolutionLevel> evolutionLevels = new List<evolutionLevel> { new evolutionLevel(10, 60, 20, new EvolveCost[] {}), new evolutionLevel(15, 100, 30, new EvolveCost[] {new EvolveCost("RedBlobDrop", 10)}) , new evolutionLevel(30, 1000, 100, new EvolveCost[] {new EvolveCost("RedBlobDrop", 10)})};
    int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // playerStatus = gameObject.GetComponent<PlayerStatus>();
        EvolveNext();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void EvolveNext() {
        evolutionLevel evolutionStats;
        if (currentIndex != evolutionLevels.Count){
        evolutionStats = evolutionLevels[currentIndex];

        for (int i = 0; i < evolutionStats.cost.Length; i++) {
            EvolveCost cost = evolutionStats.cost[i];
            if (playerStatus.invintory.ContainsKey(cost.resource) && playerStatus.invintory[cost.resource] >= cost.amount) {
            } else {
                return;
            } 
        }

        for (int i = 0; i < evolutionStats.cost.Length; i++) {
            EvolveCost cost = evolutionStats.cost[i];
            playerStatus.RemoveItemFromInvintory(cost.resource, cost.amount);
            playerStatus.BlobCountText.text = "Blob " + playerStatus.GetItemFromInvintory("RedBlobDrop");
            
        }        

        playerStatus.health = evolutionStats.mexHealth;
        playerStatus.damage = evolutionStats.damage;
        playerStatus.speed = evolutionStats.speed;
        currentIndex ++;}
    }
}





struct evolutionLevel
{
    public int damage;
    public int speed;
    public int mexHealth;

    public EvolveCost[] cost;

    public evolutionLevel(int damage, int speed, int mexHealth, EvolveCost[] cost)
    {
        this.damage = damage;
        this.speed = speed;
        this.mexHealth = mexHealth;
        this.cost = cost;
    }
}


struct EvolveCost {
    public string resource;
    public int amount;

    public EvolveCost(string resource, int amount)
    {
        this.resource = resource;
        this.amount = amount;
    }
}

