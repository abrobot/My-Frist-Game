using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainTypeSettings", menuName = "ScriptableObjects/TerrainTypeSettings", order = 1)]
public class TerrainTypeSettings : ScriptableObject
{
    [Header("BaseSettings")]
    public float scale;
    public float heightMultiplier;

    [Header("Octave Settings")]
    public int octaves;
    public float persistance;
    public float lacunarity;

    [Space(-17)]
    [Header("")]
	public TerrainRegionsType[] regions;

    [Header("ObjectPlacementSettings")]
    public TreeGenerationSetting treeGenerationSetting;

}

[System.Serializable]
public struct TerrainRegionsType {
	public string name;
	public float height;
	public Color colour;
}