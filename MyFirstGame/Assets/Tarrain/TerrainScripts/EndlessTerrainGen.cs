using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI;
using Unity.AI.Navigation;

public class EndlessTerrainGen : MonoBehaviour
{

    Dictionary<Vector2, TerrainChunk> terrainChunks = new Dictionary<Vector2, TerrainChunk>();
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


    void UpdateTerrainChunks() {
        bool changed = false;
        for (int y = -viewDistance; y <= viewDistance; y += chunkSize) {
            for (int x = -viewDistance; x <= viewDistance; x += chunkSize) {
                Vector2 chunkPos = new Vector2(viewerChunkPos.x + x, viewerChunkPos.y + y);
                if (terrainChunks.ContainsKey(chunkPos)) {
                    
                } else {
                    TerrainChunkConstructionData terrainChunkConstructionData = new TerrainChunkConstructionData(new EndlessTerrainGeneratorSettings(chunkSize, sampleGap),BaseTerrainTypeSettings, chunkPos);
                    terrainChunks.Add(chunkPos, new TerrainChunk(terrainChunkConstructionData, DefaultMaterial));
                    changed = true;
                }
            }
        }

        if (changed) {
            if (asyncOperation != null) {
                if (asyncOperation.isDone) {
                    navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
                }
            } else {
                navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
            }
        }
    }
    

    public Vector2 RoundViewerV2Pos(Vector2 viewerPos, int roundInt) {
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
        TerrainEnvironmentGenerater.treesStorageGameobject =  treesStorageGameobject;

    }


    void Update()
    {
        viewerChunkPos = RoundViewerV2Pos(new Vector2(viewer.transform.position.x, viewer.transform.position.z), chunkSize);
        UpdateTerrainChunks();
    }
}


public class EndlessTerrainGeneratorSettings {
    public int chunkSize;
    public int sampleGap;
    
    public EndlessTerrainGeneratorSettings(int chunkSize, int sampleGap) {
        this.chunkSize = chunkSize;
        this.sampleGap = sampleGap;
    }
}