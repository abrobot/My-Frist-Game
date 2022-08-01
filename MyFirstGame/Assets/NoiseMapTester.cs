using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapTester : MonoBehaviour
{


    public Material DefaultMaterial;

    public Vector3 terrainOriginPosition;
    public int viewDistance;
    public GameObject TexturePlain;

    public int chunkSize;
    public int sampleGap = 1;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;

    public bool autoUpdate;

    public TerrainRegionsType[] regions;
    public TerrainRegionsType[] desertRegions;


    // Start is called before the first frame update
    void Start()
    {
        GenerateNoiseMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    float[,] GenerateBiomeMap(float[,] highNoiseMap, float[,] temperatureNoiseMap, float[,] moistureNoiseMap)
    {
        int width = highNoiseMap.GetLength(0);
        int height = highNoiseMap.GetLength(1);

        float[,] BiomeMapMap = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                if (temperatureNoiseMap[x, y] >= .5 && moistureNoiseMap[x, y] <= .5)
                {
                    BiomeMapMap[x, y] = 2;
                }
                else
                {
                    BiomeMapMap[x, y] = 1;
                }


            }
        }
        return BiomeMapMap;
    }


    public void GenerateNoiseMap()
    {
        NoiseMapConstructionData highConstructionData = new NoiseMapConstructionData(87345345, gameObject.transform.position, chunkSize, sampleGap, scale, octaves, persistance, lacunarity);
        NoiseMapConstructionData temperatureConstructionData = new NoiseMapConstructionData(343757874, gameObject.transform.position, chunkSize, sampleGap, scale, octaves,  persistance, lacunarity);
        NoiseMapConstructionData moistureConstructionData = new NoiseMapConstructionData(763736343, gameObject.transform.position, chunkSize, sampleGap, scale, octaves,  persistance, lacunarity);


        float[,] hightNoiseMap = NoiseMapGenerator.GenerateNoiseMap(highConstructionData);
        float[,] temperatureNoiseMap = NoiseMapGenerator.GenerateNoiseMap(temperatureConstructionData);
        float[,] moistureNoiseMap = NoiseMapGenerator.GenerateNoiseMap(moistureConstructionData);

        float[,] biomeMap = GenerateBiomeMap(hightNoiseMap, temperatureNoiseMap, moistureNoiseMap);



        Texture2D texture = TextureGenerator.TextureFromColourMap(TextureGenerator.GenerateTextureDataFromRegionsSettings(hightNoiseMap, biomeMap, new TerrainRegionsType[][] {regions, desertRegions} ));

        TexturePlain.transform.localScale = new Vector3(chunkSize, .1f, chunkSize);
        TexturePlain.SetActive(true);

        TexturePlain.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
    }
}
