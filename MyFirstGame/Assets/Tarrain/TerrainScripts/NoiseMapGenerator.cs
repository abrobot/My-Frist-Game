using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGenerator
{
    public static float[,] GenerateNoiseMap(NoiseMapConstructionData NoiseMapData)
    {

        Vector2 position = NoiseMapData.position;
        int chunkSize = NoiseMapData.chunkSize + 1;
        int SampleGap = NoiseMapData.SampleGap;
        float scale = NoiseMapData.scale;

        int octaves = NoiseMapData.octaves;
        float persistance = NoiseMapData.persistance;
        float lacunarity = NoiseMapData.lacunarity;

        int halfChunkSize = chunkSize / 2;
        int VerticesPerLine = (int)Mathf.Ceil((float)chunkSize / SampleGap);

        // calculates min and max noise height. used later to normalize noise value
        (float minNoiseHeight, float maxNoiseHeight) = GetMin_MaxPossibleHeightValue(octaves, persistance);

        float[,] noiseMap = new float[VerticesPerLine, VerticesPerLine];

        for (int y = 0; y < VerticesPerLine; y++)
        {
            for (int x = 0; x < VerticesPerLine; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // loops through octaves (int value), and samples noise value at position. amplitude and frequency are modified every iteration to add vareation to the noise.
                // the octaves are then added together. this give the noise a more natural look.
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (((NoiseMapData.seed + position.x) - halfChunkSize) + x * SampleGap) / scale * frequency;
                    float sampleY = (((NoiseMapData.seed + position.y) - halfChunkSize) + y * SampleGap) / scale * frequency;

                    // received value is * by 2 and - by 1 in order to make value a number between -1 and 1. 
                    // this is done to add negitive height for water
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                // normalizes noise value back between 0 and 1
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseHeight);
            }
        }
        return noiseMap;
    }

    public static float[,] CombaineNoiseMaps(List<float[,]> noiseMaps)
    {
        int width = noiseMaps[0].GetLength(0);
        int height = noiseMaps[0].GetLength(1);

        float[,] newNoiseMap = new float[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                float CombaineHeightPoint = 0f;

                foreach (float[,] map in noiseMaps)
                {                
                    Debug.Log(map[x, y]);
                    CombaineHeightPoint += map[x, y];
                    
                }

                //Debug.Log(CombaineHeightPoint);
                //Debug.Log(Mathf.InverseLerp(0, 3, CombaineHeightPoint));
                newNoiseMap[x, y] = Mathf.InverseLerp(0, 3, CombaineHeightPoint);
                
            }
        }
        return newNoiseMap;
    }


    //calculates min and max noise height.
    static (float, float) GetMin_MaxPossibleHeightValue(int octaves, float persistance)
    {
        float amplitude = 1;
        float maxNoiseHeight = 0;
        float minNoiseHeight = 0;

        for (int i = 0; i < octaves; i++)
        {

            maxNoiseHeight += 1 * amplitude;
            minNoiseHeight += -1 * amplitude;

            amplitude *= persistance;
        }

        return (minNoiseHeight, maxNoiseHeight);
    }
}


public class NoiseMapConstructionData
{
    public Vector2 position;
    public int chunkSize;
    public int SampleGap;
    public float scale;

    public int octaves;
    public float persistance;
    public float lacunarity;
    public int seed;


    public NoiseMapConstructionData(int seed, Vector2 position, int chunkSize, int SampleGap, float scale, int octaves, float persistance, float lacunarity)
    {
        this.position = position;
        this.chunkSize = chunkSize;
        this.SampleGap = SampleGap;

        if (scale == 0)
        {
            this.scale = 0.001f;
        }
        else
        {
            this.scale = scale;
        }

        this.octaves = octaves;
        this.persistance = persistance;
        this.lacunarity = lacunarity;
        this.seed = seed;
    }
}
