using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{

    public string BaseSeed;

    [SerializeField]
    public static Dictionary<string, int> gameSeeds = new Dictionary<string, int>();
    // Start is called before the first frame update
    void Awake()
    {
        int BaseSeedAsInt = BaseSeed.GetHashCode();
        Random.InitState(BaseSeedAsInt);
        gameSeeds.Add("MainSeed", BaseSeedAsInt);
        GenerateNewSeed("Terrain");
        GenerateNewSeed("Trees1");
        GenerateNewSeed("Trees2");
    }

    void GenerateNewSeed(string name) {
        int newSeed = Random.Range(100000, 999999);
        //print(name);
        gameSeeds.Add(name, newSeed);
        //print(newSeed);
    }

}


