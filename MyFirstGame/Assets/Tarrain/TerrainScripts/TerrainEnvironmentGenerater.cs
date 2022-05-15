using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


static public class TerrainEnvironmentGenerater
{
    static LayerMask layerMask;
    public static List<GameObject> trees;
    public static GameObject treesStorageGameobject;


    static public IEnumerator GenerateEnironment(TerrainChunk terrainChunk)
    {
        if (trees == null)
        {
            trees = GetPrefabsFromFolder.Get("Trees");
        }

        yield return new WaitForSeconds(.1f);

        Game.coroutineHandler.StartCoroutine(GenerateTrees(terrainChunk));
    }

    static public IEnumerator GenerateTrees(TerrainChunk terrainChunk)
    {
        LayerMask layerMask = LayerMask.GetMask("Terrain");
        NoiseMapDataForTreePlacement generalNoiseMapSettings = terrainChunk.terrainChunkConstructionData.treeGenerationSetting.GeneralNoiseMapSettings;
        NoiseMapDataForTreePlacement specificNoiseMapSettings = terrainChunk.terrainChunkConstructionData.treeGenerationSetting.SpesificNoiseMapSettings;


        float[,] generalNoiseMap = NoiseMapGenerator.GenerateNoiseMap(new NoiseMapConstructionData(Seed.gameSeeds["Trees1"], terrainChunk.terrainChunkConstructionData.position, terrainChunk.terrainChunkConstructionData.chunkSize, generalNoiseMapSettings.resolution, generalNoiseMapSettings.scale, generalNoiseMapSettings.octaves, generalNoiseMapSettings.persistance, generalNoiseMapSettings.lacunarity));
        float[,] specificNoiseMap = NoiseMapGenerator.GenerateNoiseMap(new NoiseMapConstructionData(Seed.gameSeeds["Trees2"], terrainChunk.terrainChunkConstructionData.position, terrainChunk.terrainChunkConstructionData.chunkSize, specificNoiseMapSettings.resolution, specificNoiseMapSettings.scale, generalNoiseMapSettings.octaves, specificNoiseMapSettings.persistance, specificNoiseMapSettings.lacunarity));



        int width = generalNoiseMap.GetLength(0);
        int height = generalNoiseMap.GetLength(1);


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                if (generalNoiseMap[x, y] > .25 && specificNoiseMap[x, y] > .87)
                {
                    Vector3 treesPosition = new Vector3((terrainChunk.terrainChunkConstructionData.position.x) + (x * generalNoiseMapSettings.resolution), 100, (terrainChunk.terrainChunkConstructionData.position.y) + (y * generalNoiseMapSettings.resolution));
                    RaycastHit hit;
                    //Debug.DrawLine(treesPosition, treesPosition - new Vector3(0, 500, 0), Color.yellow, 15f);
                    while (true)
                    {

                        if (Physics.Raycast(treesPosition, Vector3.down, out hit, 500f, layerMask))
                        {
                            treesPosition.y = hit.point.y - .2f;
                            GameObject newTree = Object.Instantiate(trees[0], treesPosition, Quaternion.identity, treesStorageGameobject.transform);
                            newTree.name = newTree.name + "";
                            break;
                        }
                        yield return null;
                    }
                    //Debug.Log(hit.rigidbody +  "    //    " + treesPosition +  "    //    " + hit.rigidbody.transform.gameObject);
                }
            }
        }
    }
}
