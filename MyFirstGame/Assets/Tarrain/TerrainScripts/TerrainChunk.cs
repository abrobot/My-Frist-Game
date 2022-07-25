using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;


public partial class TerrainChunk
{

    public static bool terrainThreadRunning = false;

    public static GameObject terrainChunkStorageGameObject;
    public static Queue<DataForTerrainChunkCalculations> dataForTerrainChunkCalculationsQueue = new Queue<DataForTerrainChunkCalculations>();
    public static Queue<TerrainChunkGenerationData> terrainChunkGenerationDataQueue = new Queue<TerrainChunkGenerationData>();

    static public void ResetStaticVar() {
        terrainThreadRunning = false;
        dataForTerrainChunkCalculationsQueue = new Queue<DataForTerrainChunkCalculations>();
        terrainChunkGenerationDataQueue = new Queue<TerrainChunkGenerationData>();
    }


    Material material;
    public TerrainChunkConstructionData terrainChunkConstructionData;

    public GameObject chunkGameObject;
    public List<GameObject> chunkTrees = new List<GameObject>();


    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    MeshCollider meshCollider;
    Rigidbody rigidbody;

    float[,] noiseMap;
    MeshData meshData;
    Mesh mesh;

    public bool fullyGenerated = false;




    public TerrainChunk(TerrainChunkConstructionData terrainChunkConstructionData, Material material)
    {
        this.terrainChunkConstructionData = terrainChunkConstructionData;
        this.material = material;

        lock (dataForTerrainChunkCalculationsQueue)
        {
            DataForTerrainChunkCalculations dataForTerrainChunkCalculations = new DataForTerrainChunkCalculations(terrainChunkConstructionData);
            dataForTerrainChunkCalculationsQueue.Enqueue(dataForTerrainChunkCalculations);
        }

        //terrainChunksForBuildQueue.Enqueue(new TerrainChunkBuildInfo<TerrainChunk>(this));
        // ThreadStart terrainCalculationsThreadStart = delegate{
        //     DoTerrainCalculations(FinishTerrainChunkGeneration);
        // };

        // new Thread (terrainCalculationsThreadStart).Start();


        //noiseMap = NoiseMapGenerator.GenerateNoiseMap((NoiseMapConstructionData)terrainChunkConstructionData.terrainMeshConstructionData);
        //meshData = TerrainChunkMeshConstructer.GenerateTerrainMesh(terrainChunkConstructionData.terrainMeshConstructionData, noiseMap);

        //Texture2D texture = TextureGenerator.CreateTextureFromRegionsSettings(noiseMap, terrainChunkConstructionData.terrainTypeSettings.regions);

        //MakeChunkGameobject(this, terrainChunkConstructionData.terrainMeshConstructionData.position);
        //this.meshRenderer.material.mainTexture = texture;

        //  this.mesh = meshData.CreateMesh();
        //  this.meshFilter.mesh = mesh;
        //  this.meshCollider.sharedMesh = this.mesh;


        //Game.coroutineHandler.callCoroutine(TerrainEnvironmentGenerater.GenerateEnironment(this));
    }


    public static TerrainChunkGenerationData DoTerrainCalculations(TerrainChunkConstructionData terrainChunkConstructionData)
    {

        float[,] _noiseMap = NoiseMapGenerator.GenerateNoiseMap((NoiseMapConstructionData)terrainChunkConstructionData.terrainMeshConstructionData);
        MeshData _meshData = TerrainChunkMeshConstructer.GenerateTerrainMesh(terrainChunkConstructionData.terrainMeshConstructionData, _noiseMap);

        TerrainChunkGenerationData terrainChunkGenerationData = new TerrainChunkGenerationData();
        terrainChunkGenerationData.noiseMap = _noiseMap;
        terrainChunkGenerationData.meshData = _meshData;
        terrainChunkGenerationData.terrainTextureData = TextureGenerator.GenerateTextureDataFromRegionsSettings(_noiseMap, terrainChunkConstructionData.terrainTypeSettings.regions);

        terrainChunkGenerationData.potentialTreeLocations = TerrainChunk.DoTreeGenerationCalculations(terrainChunkConstructionData);

        return terrainChunkGenerationData;
    }



    static public void startTerrainThreadLoop()
    {
        ThreadStart terrainCalculationsThreadStart = delegate
        {
            terrainThreadRunning = true;

            while (terrainThreadRunning)
            {
                if (dataForTerrainChunkCalculationsQueue.Count > 0)
                {
                    DataForTerrainChunkCalculations dataForTerrainChunkCalculations = dataForTerrainChunkCalculationsQueue.Dequeue();
                    TerrainChunkGenerationData terrainChunkGenerationData = DoTerrainCalculations(dataForTerrainChunkCalculations.terrainChunkConstructionData);
                    terrainChunkGenerationData.identifier = dataForTerrainChunkCalculations.terrainChunkConstructionData.position;
                    terrainChunkGenerationDataQueue.Enqueue(terrainChunkGenerationData);
                }
                Thread.Sleep(150);
            }
        };

        new Thread(terrainCalculationsThreadStart).Start();
    }





    // static public void DoTerrainCalculations(TerrainChunkConstructionData terrainChunkConstructionData) {

    //     float[,] noiseMap = NoiseMapGenerator.GenerateNoiseMap((NoiseMapConstructionData)terrainChunkConstructionData.terrainMeshConstructionData);
    //     MeshData meshData = TerrainChunkMeshConstructer.GenerateTerrainMesh(terrainChunkConstructionData.terrainMeshConstructionData, noiseMap);
    //     //terrainCalculationsInfoQueue.Enqueue(new TerrainCalculations(terrainChunkConstructionData));
    // }


    static public void FinishTerrainChunkCreating(TerrainChunkGenerationData terrainChunkGenerationData, TerrainChunk terrainChunk)
    {
        terrainChunk.noiseMap = terrainChunkGenerationData.noiseMap;
        terrainChunk.meshData = terrainChunkGenerationData.meshData;

        Texture2D texture = TextureGenerator.TextureFromColourMap(terrainChunkGenerationData.terrainTextureData);

        MakeChunkGameobject(terrainChunk, terrainChunk.terrainChunkConstructionData.terrainMeshConstructionData.position);
        terrainChunk.meshRenderer.material.mainTexture = texture;

        terrainChunk.mesh = terrainChunk.meshData.CreateMesh();
        terrainChunk.meshFilter.mesh = terrainChunk.mesh;
        terrainChunk.meshCollider.sharedMesh = terrainChunk.mesh;

        Game.instance.WaitFor(terrainChunk.terrainChunkConstructionData.position.ToString(), 0.030f, () => {
             terrainChunk.PlaceTreesFromList(terrainChunkGenerationData.potentialTreeLocations);
        });
        //terrainChunk.PlaceTreesFromList(terrainChunkGenerationData.potentialTreeLocations);
        // ThreadStart GenerateEnironmentThreadStart = delegate{
        //     terrainChunk.GenerateEnironment();
        // };

        // new Thread (GenerateEnironmentThreadStart).Start();

    }




    public static void MakeChunkGameobject(TerrainChunk terrainChunk, Vector2 position)
    {
        terrainChunk.chunkGameObject = new GameObject(position.ToString());
        terrainChunk.chunkGameObject.layer = LayerMask.NameToLayer("Terrain");
        terrainChunk.meshRenderer = terrainChunk.chunkGameObject.AddComponent<MeshRenderer>();
        terrainChunk.meshRenderer.material = terrainChunk.material;
        terrainChunk.meshFilter = terrainChunk.chunkGameObject.AddComponent<MeshFilter>();

        terrainChunk.meshCollider = terrainChunk.chunkGameObject.AddComponent<MeshCollider>();
        terrainChunk.rigidbody = terrainChunk.chunkGameObject.AddComponent<Rigidbody>();
        terrainChunk.rigidbody.isKinematic = true;

        terrainChunk.chunkGameObject.transform.position = new Vector3(position.x, 0, position.y);
        terrainChunk.chunkGameObject.transform.parent = terrainChunkStorageGameObject.transform;
    }
}


public class TerrainChunkConstructionData
{
    EndlessTerrainGeneratorSettings endlessTerrainGeneratorSettings;
    public TerrainMeshConstructionData terrainMeshConstructionData;
    public TreeGenerationSetting treeGenerationSetting;
    public TerrainTypeSettings terrainTypeSettings;
    public Vector3 position;
    public int chunkSize;


    public TerrainChunkConstructionData(EndlessTerrainGeneratorSettings endlessTerrainGeneratorSettings, TerrainTypeSettings terrainTypeSettings, Vector3 chunkPosition)
    {
        this.endlessTerrainGeneratorSettings = endlessTerrainGeneratorSettings;
        this.position = chunkPosition;
        this.chunkSize = endlessTerrainGeneratorSettings.chunkSize;
        this.terrainTypeSettings = terrainTypeSettings;
        this.treeGenerationSetting = terrainTypeSettings.treeGenerationSetting;
        this.terrainMeshConstructionData = new TerrainMeshConstructionData(
            Seed.gameSeeds["Terrain"],
            this.position,
            this.chunkSize,
            endlessTerrainGeneratorSettings.sampleGap,
            terrainTypeSettings.scale,
            terrainTypeSettings.heightMultiplier,
            terrainTypeSettings.octaves,
            terrainTypeSettings.persistance,
            terrainTypeSettings.lacunarity);
    }

}


public class TerrainChunkGenerationData
{
    public Vector2 identifier;

    public float[,] noiseMap { get; set; }
    public MeshData meshData { get; set; }
    public terrainTextureData terrainTextureData; 

    public List<Vector3> potentialTreeLocations;
}

public struct DataForTerrainChunkCalculations
{
    public readonly TerrainChunkConstructionData terrainChunkConstructionData;

    public DataForTerrainChunkCalculations(TerrainChunkConstructionData terrainChunkConstructionData)
    {
        this.terrainChunkConstructionData = terrainChunkConstructionData;
    }
}
// public struct TerrainChunkGenerationData<T1, T2>{
//     public readonly Action<T1, T2> callback;
//     public readonly T1 noiseMap;
//     public readonly T2 meshData;

//     public TerrainChunkGenerationData(Action<T1, T2> callback, T1 noiseMap, T2 meshData)
//     {
//         this.callback = callback;
//         this.noiseMap = noiseMap;
//         this.meshData = meshData;
//     }
// }
