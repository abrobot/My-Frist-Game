using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "InventoryItemSO", menuName = "ScriptableObjects/InventoryItemSO", order = 1)]
public class InventoryItemSO : ScriptableObject
{

    public string itemName;
    public GameObject model;
    public Sprite sprite;

    public bool stackable;
    



    // [Header("BaseSettings")]
    // public float scale;
    // public float heightMultiplier;

    // [Header("Octave Settings")]
    // public int octaves;
    // public float persistance;
    // public float lacunarity;

    // [Space(-17)]
    // [Header("")]
	// public TerrainRegionsType[] regions;

    // [Header("ObjectPlacementSettings")]
    // public TreeGenerationSetting treeGenerationSetting;

}
