using UnityEngine;
using System.Collections;

public static class TextureGenerator {

	public static Texture2D TextureFromColourMap(terrainTextureData terrainTextureData) {
		Texture2D texture = new Texture2D (terrainTextureData.width, terrainTextureData.height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels (terrainTextureData.colourMap);
		texture.Apply ();
		return texture;
	}


	public static terrainTextureData TextureFromHeightMap(float[,] heightMap) {
		int width = heightMap.GetLength (0);
		int height = heightMap.GetLength (1);

		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, heightMap [x, y]);
			}
		}

		return new terrainTextureData(colourMap, width, height);
		//return TextureFromColourMap (colourMap, width, height);
	}


	public static terrainTextureData GenerateTextureDataFromRegionsSettings(float[,] noiseMap, TerrainRegionsType[] regions) {
		int width = noiseMap.GetLength (0);
		int height = noiseMap.GetLength (1);

		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				float currentHeight = noiseMap [x, y];
				for (int i = 0; i < regions.Length; i++) {
					if (currentHeight <= regions [i].height) {
						colourMap [y * width + x] = regions [i].colour;
						break;
					}
				}
			}
		}
		return new terrainTextureData(colourMap, width, height);
	}
}

public struct terrainTextureData {
	public Color[] colourMap;
	public int width;
	public int height;

    public terrainTextureData(Color[] colourMap, int width, int height)
    {
        this.colourMap = colourMap;
        this.width = width;
        this.height = height;
    }
}
