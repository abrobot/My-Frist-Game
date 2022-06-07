using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI;
using Unity.AI.Navigation;
using System;
using System.Threading;

public class EndlessTerrainGen : MonoBehaviour
{

    public static Dictionary<Vector2, TerrainChunk> terrainChunks = new Dictionary<Vector2, TerrainChunk>();
    GameObject terrainStorageGameObject;
    GameObject terrainChunkStorageGameObject;
    GameObject treesStorageGameobject;

    public Material DefaultMaterial;

    public Vector3 terrainOriginPosition;
    public GameObject viewer;
    public int viewDistance;

    public int chunkSize;
    public int sampleGap = 1;

    Vector2 viewerChunkPos;

    NavMeshSurface navMeshSurface;
    AsyncOperation asyncOperation;


    public TerrainTypeSettings BaseTerrainTypeSettings;


    List<Vector2> UpdateTerrainChunks()
    {
        bool changed = false;
        List<Vector2> validTerrainChunks = new List<Vector2>();

        for (int y = -viewDistance; y <= viewDistance; y += chunkSize)
        {
            for (int x = -viewDistance; x <= viewDistance; x += chunkSize)
            {
                Vector2 chunkPos = new Vector2(viewerChunkPos.x + x, viewerChunkPos.y + y);
                if (terrainChunks.ContainsKey(chunkPos))
                {

                }
                else
                {
                    TerrainChunkConstructionData terrainChunkConstructionData = new TerrainChunkConstructionData(new EndlessTerrainGeneratorSettings(chunkSize, sampleGap), BaseTerrainTypeSettings, chunkPos);
                    terrainChunks.Add(chunkPos, new TerrainChunk(terrainChunkConstructionData, DefaultMaterial));

                    changed = true;
                }
                validTerrainChunks.Add(chunkPos);
            }

        }

        if (changed)
        {
            if (asyncOperation != null)
            {
                if (asyncOperation.isDone)
                {
                    navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
                }
            }
            else
            {
                navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
            }
        }

        return validTerrainChunks;
    }


    public Vector2 RoundViewerV2Pos(Vector2 viewerPos, int roundInt)
    {
        return new Vector2(MyMathFuctions.RoundNum(viewerPos.x, roundInt), MyMathFuctions.RoundNum(viewerPos.y, roundInt));
    }


    void Start()
    {

        terrainStorageGameObject = CustomGameObject.MakeGameObject("Terrain");
        terrainChunkStorageGameObject = CustomGameObject.MakeGameObject("Chunks", default, terrainStorageGameObject);
        treesStorageGameobject = CustomGameObject.MakeGameObject("Trees", default, terrainStorageGameObject);

        navMeshSurface = terrainChunkStorageGameObject.AddComponent<NavMeshSurface>();
        navMeshSurface.collectObjects = CollectObjects.Children;
        navMeshSurface.BuildNavMesh();
        Enemy.navMeshData = navMeshSurface.navMeshData;

        TerrainChunk.terrainChunkStorageGameObject = terrainChunkStorageGameObject;
        TerrainChunk.treesStorageGameobject = treesStorageGameobject;

        TerrainChunk.layerMask = LayerMask.GetMask("Terrain");
        TerrainChunk.checkForTreePrefabs();

        TerrainChunk.startTerrainThreadLoop();

    }


    void OnApplicationQuit()
    {
        TerrainChunk.terrainThreadRunning = false;
    }


    // void startTerrainBuildThreadLoop()
    // {
    //     ThreadStart terrainCalculationsThreadStart = delegate
    //     {

    //     };

    //     new Thread(terrainCalculationsThreadStart).Start();
    // }


    void Update()
    {
        float startOfUpdate = Time.time;
        List<Vector2> validTerrainChunks = UpdateTerrainChunks();
        CleanUpInvalidTerrainChunks(validTerrainChunks);


        viewerChunkPos = RoundViewerV2Pos(new Vector2(viewer.transform.position.x, viewer.transform.position.z), chunkSize);


        if (TerrainChunk.terrainChunkGenerationDataQueue.Count > 0)
        {
            for (int i = 1; i < TerrainChunk.terrainChunkGenerationDataQueue.Count; i++)
            {
                TerrainChunkGenerationData terrainChunkGenerationData;

                lock (TerrainChunk.terrainChunkGenerationDataQueue)
                {
                    terrainChunkGenerationData = TerrainChunk.terrainChunkGenerationDataQueue.Dequeue();
                }


                TerrainChunk terrainChunk = terrainChunks[terrainChunkGenerationData.identifier];
                if (terrainChunk != null)
                {
                    TerrainChunk.FinishTerrainChunkCreating(terrainChunkGenerationData, terrainChunk);
                }
                if (Time.time - startOfUpdate > 0.0166)
                {
                    break;
                }
            }
        }

        // if (TerrainChunk.treePlaceRaycastCheckQueue.Count > 0) {
        //     for (int i = 1; i < TerrainChunk.treePlaceRaycastCheckQueue.Count; i++) {
        //         TerrainChunk.InfoForTreePlaceCheck infoForTreePlaceCheck = TerrainChunk.treePlaceRaycastCheckQueue.Dequeue();
        //         infoForTreePlaceCheck.callback(infoForTreePlaceCheck.infoForRaycast);
        //     }
        // }
    }


    public void CleanUpInvalidTerrainChunks(List<Vector2> validTerrainChunks)
    {
        List<Vector2> chunksForDestroy = new List<Vector2>();

        foreach (KeyValuePair<Vector2, TerrainChunk> item in terrainChunks)
        {
            if (item.Value.fullyGenerated)
            {
                if (validTerrainChunks.Contains(item.Key) != true)
                {
                    chunksForDestroy.Add(item.Key);
                }
            }
        }


        for (int i = 0; i < chunksForDestroy.Count; i++)
        {
            TerrainChunk terrainChunk = terrainChunks[chunksForDestroy[i]];
            terrainChunks.Remove(chunksForDestroy[i]);
            terrainChunk.ClearTrees(Destroy);
            Destroy(terrainChunk.chunkGameObject);
            terrainChunk = null;

        }
    }
}


public class EndlessTerrainGeneratorSettings
{
    public int chunkSize;
    public int sampleGap;

    public EndlessTerrainGeneratorSettings(int chunkSize, int sampleGap)
    {
        this.chunkSize = chunkSize;
        this.sampleGap = sampleGap;
    }
}