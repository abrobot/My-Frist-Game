using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


static public class TerrainEnvironmentGenerater
{
    static LayerMask layerMask;
    static List<GameObject> trees;
    public static GameObject treesStorageGameobject;


    static public IEnumerator GenerateEnironment(TerrainChunkConstructionData terrainChunkConstructionData) {
        if (trees == null) {
            trees = GetPrefabsFromFolder.Get("Trees");
        }
        yield return null;
        GenerateTrees(terrainChunkConstructionData); 
    }

    static public void GenerateTrees (TerrainChunkConstructionData terrainChunkConstructionData) {
        LayerMask layerMask = LayerMask.GetMask("Terrain");
        NoiseMapDataForTreePlacement generalNoiseMapSettings = terrainChunkConstructionData.treeGenerationSetting.GeneralNoiseMapSettings;
        NoiseMapDataForTreePlacement specificNoiseMapSettings = terrainChunkConstructionData.treeGenerationSetting.SpesificNoiseMapSettings;


        float[,] generalNoiseMap = NoiseMapGenerator.GenerateNoiseMap(new NoiseMapConstructionData(Seed.gameSeeds["Trees1"], terrainChunkConstructionData.position, terrainChunkConstructionData.chunkSize, generalNoiseMapSettings.resolution, generalNoiseMapSettings.scale, generalNoiseMapSettings.octaves, generalNoiseMapSettings.persistance, generalNoiseMapSettings.lacunarity));
        float[,] specificNoiseMap = NoiseMapGenerator.GenerateNoiseMap(new NoiseMapConstructionData(Seed.gameSeeds["Trees2"], terrainChunkConstructionData.position, terrainChunkConstructionData.chunkSize, specificNoiseMapSettings.resolution, specificNoiseMapSettings.scale, generalNoiseMapSettings.octaves, specificNoiseMapSettings.persistance, specificNoiseMapSettings.lacunarity));


        
        int width = generalNoiseMap.GetLength (0);
		int height = generalNoiseMap.GetLength (1); 


        for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {

                if (generalNoiseMap[x, y] > .25 && specificNoiseMap[x, y] > .87) {
                    Vector3 treesPosition = new Vector3((terrainChunkConstructionData.position.x) + (x * generalNoiseMapSettings.resolution), 100, (terrainChunkConstructionData.position.y) + (y * generalNoiseMapSettings.resolution));
                    RaycastHit hit;

                    if (Physics.Raycast(treesPosition, Vector3.down, out hit, 500f, layerMask)) {
                        treesPosition.y = hit.point.y - .2f;
                        Object.Instantiate(trees[0], treesPosition, Quaternion.identity, treesStorageGameobject.transform);
                    }
                }

            }
        }
    }
}
