using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk
{
    public static GameObject terrainChunkStorageGameObject;

    Material material;
    TerrainChunkConstructionData terrainChunkConstructionData;
    
    GameObject chunkGameObject;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    MeshCollider meshCollider;
    Rigidbody rigidbody;

    float[,] noiseMap;
    MeshData meshData;
    Mesh mesh;



    public TerrainChunk(TerrainChunkConstructionData terrainChunkConstructionData, Material material) {
        this.terrainChunkConstructionData = terrainChunkConstructionData;
        this.material = material;

         noiseMap = NoiseMapGenerator.GenerateNoiseMap((NoiseMapConstructionData)terrainChunkConstructionData.terrainMeshConstructionData);
         meshData = TerrainChunkMeshConstructer.GenerateTerrainMesh(terrainChunkConstructionData.terrainMeshConstructionData, noiseMap);

        Texture2D texture = TextureGenerator.CreateTextureFromRegionsSettings(noiseMap, terrainChunkConstructionData.terrainTypeSettings.regions);
        
        MakeChunkGameobject(this, terrainChunkConstructionData.terrainMeshConstructionData.position);
        this.meshRenderer.material.mainTexture = texture;

         this.mesh = meshData.CreateMesh();
         this.meshFilter.mesh = mesh;
         this.meshCollider.sharedMesh = this.mesh;

        
        Game.coroutineHandler.callCoroutine(TerrainEnvironmentGenerater.GenerateEnironment(terrainChunkConstructionData));
    }




    public static void MakeChunkGameobject (TerrainChunk terrainChunk, Vector2 position){
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


public class TerrainChunkConstructionData{
    EndlessTerrainGeneratorSettings endlessTerrainGeneratorSettings;
    public TerrainMeshConstructionData terrainMeshConstructionData;
    public TreeGenerationSetting treeGenerationSetting;
    public TerrainTypeSettings terrainTypeSettings;
    public Vector3 position;
    public int chunkSize;


    public TerrainChunkConstructionData(EndlessTerrainGeneratorSettings endlessTerrainGeneratorSettings, TerrainTypeSettings terrainTypeSettings, Vector3 chunkPosition) {
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



