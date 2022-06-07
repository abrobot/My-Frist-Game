using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Threading;



public partial class TerrainChunk
{
    public static LayerMask layerMask;
    public static List<GameObject> treePrefabs;
    public static GameObject treesStorageGameobject;
    //public static Queue<InfoForTreePlaceCheck> treePlaceRaycastCheckQueue = new Queue<InfoForTreePlaceCheck>();
    //public static List<Vector3> TreesForSecondPlaceCheck = new List<Vector3>();
    //public static List<Vector3> TreesHadSecondChance = new List<Vector3>();



    public List<GameObject> TreesForChunk = new List<GameObject>();



    public static List<Vector3> DoTreeGenerationCalculations(TerrainChunkConstructionData terrainChunkConstructionData)
    {
        NoiseMapDataForTreePlacement generalNoiseMapSettings = terrainChunkConstructionData.treeGenerationSetting.GeneralNoiseMapSettings;
        NoiseMapDataForTreePlacement specificNoiseMapSettings = terrainChunkConstructionData.treeGenerationSetting.SpesificNoiseMapSettings;

        float[,] generalNoiseMap = NoiseMapGenerator.GenerateNoiseMap(new NoiseMapConstructionData(Seed.gameSeeds["Trees1"], terrainChunkConstructionData.position, terrainChunkConstructionData.chunkSize, generalNoiseMapSettings.resolution, generalNoiseMapSettings.scale, generalNoiseMapSettings.octaves, generalNoiseMapSettings.persistance, generalNoiseMapSettings.lacunarity));
        float[,] specificNoiseMap = NoiseMapGenerator.GenerateNoiseMap(new NoiseMapConstructionData(Seed.gameSeeds["Trees2"], terrainChunkConstructionData.position, terrainChunkConstructionData.chunkSize, specificNoiseMapSettings.resolution, specificNoiseMapSettings.scale, generalNoiseMapSettings.octaves, specificNoiseMapSettings.persistance, specificNoiseMapSettings.lacunarity));



        int width = generalNoiseMap.GetLength(0);
        int height = generalNoiseMap.GetLength(1);

        List<Vector3> potentialTreeLocations = new List<Vector3>();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                if (generalNoiseMap[x, y] > .25 && specificNoiseMap[x, y] > .87)
                {
                    Vector3 treesPosition = new Vector3((terrainChunkConstructionData.position.x) + (x * generalNoiseMapSettings.resolution), 100, (terrainChunkConstructionData.position.y) + (y * generalNoiseMapSettings.resolution));
                    potentialTreeLocations.Add(treesPosition);
                }
            }
        }
        return potentialTreeLocations;
    }



    public void PlaceTreesFromList(List<Vector3> trees)
    {
       // trees.AddRange(TreesForSecondPlaceCheck);
        //TreesForSecondPlaceCheck.Clear();

        for (int i = 0; i < trees.Count; i++)
        {
            Vector3 treePosition = trees[i];
            RaycastHit hit;

            if (Physics.Raycast(treePosition, Vector3.down, out hit, 500f, layerMask))
            {
                treePosition.y = hit.point.y - .2f;
                GameObject newTree = UnityEngine.Object.Instantiate(treePrefabs[0], treePosition, Quaternion.identity, treesStorageGameobject.transform);
                newTree.name = newTree.name + "";
                TreesForChunk.Add(newTree);
            }
            // else
            // {
            //     if (!TreesHadSecondChance.Contains(treePosition))
            //     {
            //         TreesForSecondPlaceCheck.Add(treePosition);
            //         TreesHadSecondChance.Add(treePosition);
            //     } else {
            //         TreesHadSecondChance.Remove(treePosition);
            //     }
            // }
        }
        fullyGenerated = true;
    }


    public void ClearTrees (Action<GameObject> destroy) {
        for (int i = 0; i < TreesForChunk.Count; i++) {
            destroy(TreesForChunk[i]);
        }
    }


    public void GenerateEnironment()
    {

        //  waits to fix weird bug ...  yield return new WaitForSeconds(.1f);
        // GenerateTrees();
    }

    // public void GenerateTrees(TerrainChunkGenerationData)
    // {
    // NoiseMapDataForTreePlacement generalNoiseMapSettings = this.terrainChunkConstructionData.treeGenerationSetting.GeneralNoiseMapSettings;
    // NoiseMapDataForTreePlacement specificNoiseMapSettings = this.terrainChunkConstructionData.treeGenerationSetting.SpesificNoiseMapSettings;


    // float[,] generalNoiseMap = NoiseMapGenerator.GenerateNoiseMap(new NoiseMapConstructionData(Seed.gameSeeds["Trees1"], this.terrainChunkConstructionData.position, this.terrainChunkConstructionData.chunkSize, generalNoiseMapSettings.resolution, generalNoiseMapSettings.scale, generalNoiseMapSettings.octaves, generalNoiseMapSettings.persistance, generalNoiseMapSettings.lacunarity));
    // float[,] specificNoiseMap = NoiseMapGenerator.GenerateNoiseMap(new NoiseMapConstructionData(Seed.gameSeeds["Trees2"], this.terrainChunkConstructionData.position, this.terrainChunkConstructionData.chunkSize, specificNoiseMapSettings.resolution, specificNoiseMapSettings.scale, generalNoiseMapSettings.octaves, specificNoiseMapSettings.persistance, specificNoiseMapSettings.lacunarity));



    // int width = generalNoiseMap.GetLength(0);
    // int height = generalNoiseMap.GetLength(1);


    // for (int y = 0; y < height; y++)
    // {
    //     for (int x = 0; x < width; x++)
    //     {

    //         if (generalNoiseMap[x, y] > .25 && specificNoiseMap[x, y] > .87)
    //         {
    //             Vector3 treesPosition = new Vector3((this.terrainChunkConstructionData.position.x) + (x * generalNoiseMapSettings.resolution), 100, (this.terrainChunkConstructionData.position.y) + (y * generalNoiseMapSettings.resolution));
    //             lock (treePlaceRaycastCheckQueue)
    //             {
    //                 treePlaceRaycastCheckQueue.Enqueue(new InfoForTreePlaceCheck(CheckTreePlace, new InfoForRaycast(treesPosition, Vector3.down, 500f, layerMask)));
    //             };
    //         }
    //     }
    // }
    // }

    // void CheckTreePlace(InfoForRaycast infoForRaycast)
    // {
    //     RaycastHit hit;
    //     Physics.Raycast(infoForRaycast.origin, infoForRaycast.direction, out hit, infoForRaycast.maxDistance, infoForRaycast.layerMaskToCheckAgents);
    //     infoForRaycast.origin.y = hit.point.y - .2f;
    //     GameObject newTree = UnityEngine.Object.Instantiate(treePrefabs[0], infoForRaycast.origin, Quaternion.identity, treesStorageGameobject.transform);
    //     newTree.name = newTree.name + "";
    // }


    // public struct InfoForTreePlaceCheck
    // {
    //     public readonly Action<InfoForRaycast> callback;
    //     public readonly InfoForRaycast infoForRaycast;

    //     public InfoForTreePlaceCheck(Action<InfoForRaycast> raycastFinishCallback, InfoForRaycast infoForRaycast)
    //     {
    //         this.callback = raycastFinishCallback;
    //         this.infoForRaycast = infoForRaycast;
    //     }
    // }


    // public struct InfoForRaycast
    // {
    //     public Vector3 origin;
    //     public Vector3 direction;
    //     public float maxDistance;
    //     public LayerMask layerMaskToCheckAgents;

    //     public InfoForRaycast(Vector3 origin, Vector3 direction, float maxDistance, LayerMask layerMaskToCheckAgents)
    //     {
    //         this.origin = origin;
    //         this.direction = direction;
    //         this.maxDistance = maxDistance;
    //         this.layerMaskToCheckAgents = layerMaskToCheckAgents;
    //     }
    // }

    static public void checkForTreePrefabs()
    {
        if (TerrainChunk.treePrefabs == null)
        {
            TerrainChunk.treePrefabs = GetPrefabsFromFolder.Get("Trees");
        }
    }
}
