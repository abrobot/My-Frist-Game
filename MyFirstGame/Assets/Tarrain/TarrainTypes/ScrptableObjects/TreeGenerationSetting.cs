using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreeGenerationSetting", menuName = "ScriptableObjects/TreeGenerationSetting", order = 1)]
public class TreeGenerationSetting : ScriptableObject
{

    public NoiseMapDataForTreePlacement GeneralNoiseMapSettings;
    public NoiseMapDataForTreePlacement SpesificNoiseMapSettings;
}

[System.Serializable]
public struct NoiseMapDataForTreePlacement {
    public float scale;
    public int resolution;

    public int octaves;
    public float persistance;
    public float lacunarity;
}